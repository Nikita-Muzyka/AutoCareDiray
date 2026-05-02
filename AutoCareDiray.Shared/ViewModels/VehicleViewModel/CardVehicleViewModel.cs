using AutoCareDiray.Shared.Interface;
using AutoCareDiray.Shared.Models.Notes;
using AutoCareDiray.Shared.Models.RepairModel;
using AutoCareDiray.Shared.Models.VehicleModel;
using AutoCareDiray.Shared.Service.Data;
using AutoCareDiray.Shared.Service.ResultService;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AutoCareDiray.Shared.ViewModels.VehicleViewModel
{
    public partial class CardVehicleViewModel : BaseViewModel
    {
        #region Основые классы и списки

        private int _vehicleId;
        private bool _isInitilize = false;
        private bool _isNoteUpdate = false;
        private int _noteId = 0;
        private VehicleNotes _noteForUpdate;
        CancellationTokenSource _cts;

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
        private string buttonName = "Добавить";

        #endregion

        public CardVehicleViewModel(IDialogService dialog, IDataService data, INavigationService navigate)
            : base(dialog, data, navigate)
        {
            _cts = new CancellationTokenSource();
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
        public async void CreatePdfStatVehicle() //create pdf
        {
            
        }


        private async Task LoadVehicleAsync()
        {
            var result = await _dataService.GetVehicleAsync(_vehicleId, _cts.Token);
            if(result.Success)
            {
                var resultVehicle = result as Result<Vehicle>;
                VehicleCard = resultVehicle.Data ?? new Vehicle();
                var sortRepairType = VehicleCard.RepairTypes.Where(c => c.IntervalMileage > 0).ToList();
                var warning = sortRepairType.Where(c => VehicleCard.Mileage - c.LastServiceMileage > c.IntervalMileage).ToList();
                WarningRepairType = new ObservableCollection<RepairType>(warning);
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

    }
}
