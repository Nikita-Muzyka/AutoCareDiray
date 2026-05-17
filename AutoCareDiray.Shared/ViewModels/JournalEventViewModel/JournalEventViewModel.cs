using AutoCareDiray.Shared.Interface;
using AutoCareDiray.Shared.Models.Journal;
using AutoCareDiray.Shared.Models.RepairModel;
using AutoCareDiray.Shared.Models.VehicleModel;
using AutoCareDiray.Shared.Service.ResultService;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace AutoCareDiray.Shared.ViewModels.JournalEventViewModel
{
    public partial class JournalEventViewModel : BaseViewModel
    {
        #region основные классы

        private CancellationTokenSource _cts;

        #endregion

        #region основные классы для работы UI

        public ObservableCollection<Vehicle> Vehicles { get; set; }
        public ObservableCollection<TimelineEvent> JournalEvents { get; set; } = new ObservableCollection<TimelineEvent>();

        [ObservableProperty]
        private Vehicle selectedVehicle;

        [ObservableProperty]
        private bool isEnableButtonCreate;

        #endregion

        public JournalEventViewModel(IDialogService dialog, IDataService data, INavigationService navigate) : base(dialog,data,navigate)
        {
            _cts = new CancellationTokenSource();
        }

        async partial void OnSelectedVehicleChanged(Vehicle value)
        {
            if(value != null)
            {
                IsEnableButtonCreate = true;
                await ShowJournal();
            }
            else IsEnableButtonCreate = false;
        }

        private async Task ShowJournal()
        {
            var result = await _dataService.GetFullVehicleAsync(SelectedVehicle.Id, _cts.Token);
            if (result.Success)
            {
                var resultVehicle = result as Result<Vehicle>;
                ObservableCollection<TimelineEvent> list = new ObservableCollection<TimelineEvent>();

                if (resultVehicle.Data.Repairs == null || resultVehicle.Data.Repairs.Count == 0) { }
                else
                {
                    foreach (var repair in resultVehicle.Data.Repairs)
                    {
                        list.Add(new TimelineEvent
                        {
                            RecordId = repair.Id,
                            Type = EventType.Repair,
                            Date = repair.DateRepair,
                            Title = repair.RepairType.TitleRepair,
                            Cost = repair.Cost,
                            Mileage = repair.CurrentMileage + " км. "
                        });
                    }
                }

                if (resultVehicle.Data.Refills == null || resultVehicle.Data.Refills.Count == 0) { }
                else
                {
                    foreach (var refill in resultVehicle.Data.Refills)
                    {
                        list.Add(new TimelineEvent
                        {
                            RecordId = refill.Id,
                            Type = EventType.Refill,
                            Date = refill.DateRefill,
                            Title = refill.Title,
                            Cost = refill.Cost,
                            Subtitle = refill.VolumeLiters + " л. ",
                            Mileage = refill.Mileage + " км. "
                        });
                    }
                }

                JournalEvents = new ObservableCollection<TimelineEvent>(list.OrderByDescending(e => e.Date));
                OnPropertyChanged(nameof(JournalEvents));
            }
        }

        [RelayCommand]
        public async Task Initilize()
        {
           var result = await _dataService.GetListVehicleNameAsync(_cts.Token);
            if (result.Success)
            {
                var resultVehicles = result as Result<List<Vehicle>>;
                Vehicles = new ObservableCollection<Vehicle>(resultVehicles.Data);
                OnPropertyChanged(nameof(Vehicles));
            }
        }

        [RelayCommand]
        public async Task ShowMenuAdd()
        {
            var result = await _dialogService.ShowDisplayAddMenu();

            var property = new Dictionary<string, object>
                    {
                        { "VehicleId", SelectedVehicle.Id }
                    };
            switch (result)
            {

                case "⛽ Заправку":
                    await _navigationService.GoNavigation("CreateRefillView",property);
                    break;

                case "🛠 Ремонт":
                    await _navigationService.GoNavigation("CreateRepairView",property);
                    break;

                case "🧾 Прочий расход":
                    // Переходим на страницу расхода (мойка, страховка)
                    break;
            }
        }

        [RelayCommand]
        public async Task ShowEventOptions(TimelineEvent selectedTimeLine)
        {
            if (selectedTimeLine == null) return;
            var respon = await _dialogService.ShowDisplayAction();
            if (respon == "Отмена") return;
            if (respon == "Редактировать")
            {
                if(selectedTimeLine.Type == EventType.Repair)
                {
                    await _navigationService.GoNavigation("CreateRepairView", new Dictionary<string, object>
                    {
                        { "RepairId", selectedTimeLine.RecordId }
                    });
                }
                else if(selectedTimeLine.Type == EventType.Refill)
                {
                    await _navigationService.GoNavigation("CreateRefillView", new Dictionary<string, object>
                    {
                        { "RefillId", selectedTimeLine.RecordId }
                    });
                }
            }
            else if (respon == "Удалить")
            {
                if (selectedTimeLine.Type == EventType.Repair)
                {
                    var result = await _dataService.DeleteRepairAsync(selectedTimeLine.RecordId, _cts.Token);
                    if (result.Success)
                    {
                        JournalEvents.Remove(selectedTimeLine);
                    }
                }
                else if (selectedTimeLine.Type == EventType.Refill)
                {
                    var result = await _dataService.DeleteRefillAsync(selectedTimeLine.RecordId, _cts.Token);
                    if (result.Success)
                    {
                        JournalEvents.Remove(selectedTimeLine);
                    }
                }
            }
        } //Кебабб меню вывод после нажатия

        [RelayCommand]
        public async Task EventTapped(TimelineEvent selectedTimeLine)
        {
            if (selectedTimeLine == null) return;
            
                if (selectedTimeLine.Type == EventType.Repair)
                {
                    await _navigationService.GoNavigation("CardRepairView", new Dictionary<string, object>
                    {
                        { "RepairId", selectedTimeLine.RecordId }
                    });
                }
                else if (selectedTimeLine.Type == EventType.Refill)
                {
                await _navigationService.GoNavigation("CardRefillView", new Dictionary<string, object>
                    {
                        { "RefillId", selectedTimeLine.RecordId }
                    });
            }
            
            
        } //Кебабб меню вывод после нажатия

        [RelayCommand]
        public async void ShowRepairOptions(Repair selectedRepair)
        {
            if (selectedRepair == null) return;
            var respon = await _dialogService.ShowDisplayAction();
            if (respon == "Отмена") return;
            if (respon == "Редактировать") ;
            else if (respon == "Удалить") ;
        } //Кебабб меню вывод после нажатия

        [RelayCommand]
        public void CancelToken()
        {
            _cts.Cancel();
            _cts.Dispose();
            _cts = new CancellationTokenSource();
        } //отмена токена при закртие страницы
    }
}
