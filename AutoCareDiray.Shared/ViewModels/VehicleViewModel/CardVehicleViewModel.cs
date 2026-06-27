using AutoCareDiray.Shared.Interface;
using AutoCareDiray.Shared.Models.Notes;
using AutoCareDiray.Shared.Models.PredictionData;
using AutoCareDiray.Shared.Models.RepairModel;
using AutoCareDiray.Shared.Models.VehicleModel;
using AutoCareDiray.Shared.Service.Data;
using AutoCareDiray.Shared.Service.IntervalCalculator;
using AutoCareDiray.Shared.Service.ResultService;
using AutoCareDiray.Shared.Service.PredicateDateService;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using static AutoCareDiray.Shared.Models.PredictionData.PredictionDataModel;


namespace AutoCareDiray.Shared.ViewModels.VehicleViewModel
{
    public partial class CardVehicleViewModel : BaseViewModel
    {
        #region Основые классы и списки

        IPdfService _pdfService;
        private int _vehicleId;
        private bool _isInitilize = false;
        private bool _isNoteUpdate = false;
        private int _noteId = 0;
        private VehicleNotes _noteForUpdate;
        CancellationTokenSource _cts;

        private DateTime _startDate;

        #endregion

        #region Классы для UI

        [ObservableProperty]
        private ObservableCollection<RepairType> warningRepairType = new();
        [ObservableProperty]
        private ObservableCollection<VehicleNotes> vehicleNotes = new();

        [ObservableProperty]
        private Vehicle vehicleCard;

        [ObservableProperty]
        private string selectedMode;
        [ObservableProperty]
        private string newTitleNote;
        [ObservableProperty]
        private string newContentNote;
        [ObservableProperty]
        private string selectedYearReport;
        [ObservableProperty]
        private string buttonName = "Добавить";
        [ObservableProperty]
        private string mileageConvert;
        [ObservableProperty]
        private string fuelTankConvert;




        [ObservableProperty]
        private ObservableCollection<RepairType> allTrackedRepairTypes = new(); // А это для Picker'а нейросети
        [ObservableProperty]
        private string predictionText = "ИИ: расчет...";
        [ObservableProperty]
        private double wearProgress = 0.0;
        [ObservableProperty]
        private bool _isBusy;
        [ObservableProperty]
        private RepairType selectedRepairTypeForPrediction;

        #endregion

        public CardVehicleViewModel(IDialogService dialog, IDataService data, INavigationService navigate,IPdfService pdf)
            : base(dialog, data, navigate)
        {
            _cts = new CancellationTokenSource();
            _pdfService = pdf;
        }

        [RelayCommand]
        public async Task InitilizeAsync(int vehicleId)
        {
            if (_isInitilize) return;
            _vehicleId = vehicleId;

            await LoadVehicleAsync();

            // Фоновый прогрев ИИ
            _ = Task.Run(async () =>
            {
                var repairsResult = await _dataService.GetListRepairForVehicleAsync(_vehicleId, _cts.Token);
                if (repairsResult.Success)
                {
                    var repairs = (repairsResult as Result<List<Repair>>).Data;
                    var list = new List<PredictionDataModel.RepairData>();

                    // 1. РЕАЛЬНАЯ ИСТОРИЯ
                    var validRealRepairs = repairs.Where(r => r.RepairType != null && r.RepairType.IntervalMileage > 0);
                    foreach (var repair in validRealRepairs)
                    {
                        list.Add(new PredictionDataModel.RepairData
                        {
                            Mileage = (float)repair.CurrentMileage, // Пробег В МОМЕНТ ремонта
                            Cost = (float)repair.Cost,
                            RepairTypeId = repair.RepairTypeId.ToString(),
                            // УЧИМ ИИ: На каком пробеге наступит СЛЕДУЮЩЕЕ ТО
                            Label = (float)(repair.CurrentMileage + repair.RepairType.IntervalMileage)
                        });
                    }

                    var service = new PredictionService();
                    service.PrepareAndTrain(list);
                }
            });

            MileageConvert = VehicleCard.Mileage.ToString() + " " +VehicleCard.UnitDistance;
            FuelTankConvert = VehicleCard.FuelTank.ToString() + " " + VehicleCard.UnitVolume;
            _isInitilize = true;
        }

        [RelayCommand]
        public async Task UpdatePredictionAsync()
        {
            if (AllTrackedRepairTypes == null || !AllTrackedRepairTypes.Any())
            {
                await _dialogService.ShowToastAsync("Нет данных о типах ремонта.");
                return;
            }

            IsBusy = true;
            PredictionText = "ИИ: анализирую паттерны износа...";

            var predictionResult = await Task.Run(() =>
            {
                var service = new PredictionService();
                var predictions = new List<(string Category, float RemainingKm, float ExpectedMileage)>();

                var validTypesToPredict = AllTrackedRepairTypes
                    .Where(rt => rt.IntervalMileage > 0)
                    .DistinctBy(r => r.Category)
                    .ToList();

                foreach (var repairType in validTypesToPredict)
                {
                    // БЕРЕМ ПРОБЕГ ПОСЛЕДНЕЙ ЗАМЕНЫ (если еще не меняли, считаем от 0)
                    float lastServiceMileage = repairType.LastServiceMileage > 0 ? (float)repairType.LastServiceMileage : 0f;

                    // СПРАШИВАЕМ ИИ: "Мы поменяли деталь на пробеге lastServiceMileage. На каком пробеге менять снова?"
                    float expectedNextService = service.PredictNext(lastServiceMileage, 0f, repairType.Id.ToString());

                    if (expectedNextService > 0)
                    {
                        // ОСТАТОК = Пробег будущего ТО минус ТЕКУЩИЙ пробег машины
                        float remainingKm = expectedNextService - (float)VehicleCard.Mileage;

                        predictions.Add((repairType.CategoryText, remainingKm, expectedNextService));
                    }
                }

                if (!predictions.Any()) return "ИИ: Недостаточно данных для прогноза.";

                // Сортируем: сначала те, что нужно менять срочно (включая просроченные)
                var sortedPredictions = predictions.OrderBy(p => p.RemainingKm).ToList();
                var sb = new System.Text.StringBuilder();
                sb.AppendLine($"📊 Текущий пробег: {VehicleCard.Mileage:N0} км");
                sb.AppendLine("──────────────────────────────");
                sb.AppendLine("🔮 ПРОГНОЗ БЛИЖАЙШИХ ТО:");
                sb.AppendLine();

                foreach (var pred in sortedPredictions)
                {
                    // Красивая обработка просроченных ремонтов (если остаток ушел в минус)
                    string status = pred.RemainingKm < 0 ? "⚠️ ПРОСРОЧЕНО НА:" : "⏳ Через:";
                    float absRemaining = Math.Abs(pred.RemainingKm); // Берем число по модулю (без минуса)

                    sb.AppendLine($"🔧 {pred.Category}");
                    sb.AppendLine($"   {status} {absRemaining:N0} км");
                    sb.AppendLine($"   🚩 Плановый пробег ТО: {pred.ExpectedMileage:N0} км");
                    sb.AppendLine();
                }

                return sb.ToString().TrimEnd();
            });

            PredictionText = predictionResult;
            IsBusy = false;
        } // Работа с моделью формирования отчета

        [RelayCommand]
        public async Task interactionNoteAsync()
        {

            var currentNote = _isNoteUpdate ? _noteForUpdate : new VehicleNotes
            {
                VehicleId = _vehicleId,
                DateCreated = DateTime.Now,
            };

            currentNote.Title = NewTitleNote;
            currentNote.Content = NewContentNote;
            currentNote.DateUpdated = DateTime.Now;

            var result = _isNoteUpdate
                ? await _dataService.UpdateVehileNoteAsync(currentNote, _cts.Token)
                : await _dataService.CreateVehicleNotesAsync(currentNote, _cts.Token);


            if (result.Success)
            {
                if (_isNoteUpdate)
                {
                    ButtonName = "Добавить";
                    _isNoteUpdate = false;
                    _noteId = 0;
                    await _dialogService.ShowToastAsync("Заметка обновлена");
                }
                else
                {
                    VehicleNotes.Add(currentNote);
                    await _dialogService.ShowToastAsync("Заметка создана");
                }
                NewTitleNote = string.Empty;
                NewContentNote = string.Empty;
            }
            else
            {
                await _dialogService.ShowToastAsync(result.ErrorMessage);
            }
        } //Создание заметок

        [RelayCommand]
        public async void ShowNotesOptions(VehicleNotes selectedNotes)
        {
            if (selectedNotes == null) return;
            var respon = await _dialogService.ShowDisplayAction();
            if (respon == "Отмена") return;
            if (respon == "Редактировать") await EditNotesAsync(selectedNotes);
            else if (respon == "Удалить") await DeleteNotesAsync(selectedNotes);
        }

        [RelayCommand]
        public async void CreatePdfStatVehicle() 
        {
            if(VehicleCard.PdfFile != null)
            {
                _pdfService.DeletePdf(VehicleCard.PdfFile);
            }
            var result = await _pdfService.CreatePdfStateAsync(VehicleCard, _startDate, DateTime.UtcNow);
            if (result.Success == false)
            {
                await _dialogService.ShowToastAsync(result.ErrorMessage);
                return;
            }

            var resultFile = result as Result<string>;
            var resultSave = await _dialogService.ShowChoiceDisplayAlertAsync("Сохранение", "Вы хотите сохранить данный PDF ?", "Да", "Нет");
            if (resultSave)
            {
                VehicleCard.PdfFile = resultFile.Data;
                var resultUpdate = await _dataService.UpdateVehiclePdfAsync(VehicleCard.Id,VehicleCard.PdfFile,_cts.Token);
                if(resultUpdate.Success)
                {
                    await _dialogService.ShowToastAsync("PDF сохранен");
                }
                else
                {
                    await _dialogService.ShowToastAsync("Ошибка при сохранении PDF");
                }
            }
            else
            {
               _pdfService.DeletePdf(resultFile.Data);
            }
        } //create pdf

        [RelayCommand]
        public async void OpenPdfFile()
        {
            await _pdfService.OpenPdfAsync(vehicleCard.PdfFile);
        } // открыть пдф

        private async Task LoadVehicleAsync()
        {
            var result = await _dataService.GetVehicleAsync(_vehicleId, _cts.Token);
            if (result.Success)
            {
                var resultVehicle = result as Result<Vehicle>;
                VehicleCard = resultVehicle.Data ?? new Vehicle();

               
                var resultWarning = IntervalCalculatroService.CalculatingWarningList(VehicleCard);
                WarningRepairType.Clear();
                if (resultWarning != null)
                {
                    foreach (var item in resultWarning)
                        WarningRepairType.Add(item);
                }

                // 2. Заполняем список ВООБЩЕ ВСЕХ деталей (для выпадающего списка ИИ)
                AllTrackedRepairTypes.Clear();
                if (VehicleCard.RepairTypes != null)
                {
                    foreach (var item in VehicleCard.RepairTypes)
                        AllTrackedRepairTypes.Add(item);
                }
            }
        }

        async partial void OnSelectedModeChanged(string value)
        {
            if (value == "Notes")
            {
                var result = await _dataService.GetListVehicleNotesAsync(_vehicleId, _cts.Token);
                if (result.Success)
                {
                    var resultNotes = result as Result<List<VehicleNotes>>;
                    VehicleNotes.Clear();
                    foreach(var notes in resultNotes.Data)
                    {
                        VehicleNotes.Add(notes);
                    }
                }
            }
        } //логика инициализации заметки

        private async Task EditNotesAsync(VehicleNotes note)
        {
            if (_isNoteUpdate)
            {
                if(note.Id == _noteId)
                {
                    await _dialogService.ShowToastAsync("Вы уже редактируете эту заметку");
                    return;
                }
            }
            _isNoteUpdate = true;
            _noteId = note.Id;
            _noteForUpdate = note;
            ButtonName = "Изменить";
            NewTitleNote = note.Title;
            NewContentNote = note.Content;
        }

        private async Task DeleteNotesAsync(VehicleNotes note)
        {
            var result = await _dataService.DeleteVehicleNotesAsync(note.Id,_cts.Token);
            VehicleNotes.Remove(note);
            if (result.Success) await _dialogService.ShowToastAsync("Заметка удалена");
        }
        partial void OnSelectedYearReportChanged(string value)
        {
            if(value == "NowYear")
            {
                DateTime nowDate = DateTime.UtcNow;
                _startDate = new DateTime(nowDate.Year, 1, 1);
            }
            if(value == "PastYear")
            {
                DateTime nowDate = DateTime.UtcNow;
                _startDate = new DateTime(nowDate.Year - 1, 1, 1);
            }
            else
            {
                    _startDate = vehicleCard.YearPurchase;
            }
        }

        [RelayCommand]
        public void CancelToken()
        {
            _cts.Cancel();
            _cts.Dispose();
            _cts = new CancellationTokenSource();
        }  //отмена токена

      
    }
}
