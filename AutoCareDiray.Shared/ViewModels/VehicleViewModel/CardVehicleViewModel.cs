using AutoCareDiray.Shared.Interface;
using AutoCareDiray.Shared.Models.Notes;
using AutoCareDiray.Shared.Models.RepairModel;
using AutoCareDiray.Shared.Models.VehicleModel;
using AutoCareDiray.Shared.Service.Data;
using AutoCareDiray.Shared.Service.IntervalCalculator;
using AutoCareDiray.Shared.Service.ResultService;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.IO;

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
        private ObservableCollection<RepairType> warningRepairType;
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
            _isInitilize = true;
        } // инициализация карточки
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
            if (result.Success == false) await _dialogService.ShowToastAsync(result.ErrorMessage);
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
        }


        private async Task LoadVehicleAsync()
        {
            var result = await _dataService.GetVehicleAsync(_vehicleId, _cts.Token);
            if(result.Success)
            {
                var resultVehicle = result as Result<Vehicle>;
                VehicleCard = resultVehicle.Data ?? new Vehicle();
                var resultWarning = IntervalCalculatroService.CalculatingWarningList(VehicleCard);
                WarningRepairType = new ObservableCollection<RepairType>(resultWarning);
            }
        } // загрузка информации по авто

        async partial void OnSelectedModeChanged(string value)
        {
            if (value == "Notes")
            {
                var result = await _dataService.ListVehicleNotesAsync(_vehicleId, _cts.Token);
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
                if(vehicleCard.YearPurchase != null)
                {
                    _startDate = vehicleCard.YearPurchase;
                }
            }
        }

    }
}
