using AutoCareDiray.Shared.Extensions.EventTypeEx;
using AutoCareDiray.Shared.Extensions.RepairEx;
using AutoCareDiray.Shared.Extensions.StringEx;
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
        private CancellationTokenSource _searchcts;
        private readonly IMainThreadService _mainThreadService;

        #endregion

        #region основные классы для работы UI

        public ObservableCollection<Vehicle> Vehicles { get; set; }
        private List<TimelineEvent> _journalDB;
        public ObservableCollection<TimelineEvent> JournalEvents { get; set; } = new ObservableCollection<TimelineEvent>();
        public ObservableCollection<string> RepairCategories { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> EventTypes { get; set; } = new ObservableCollection<string>();

        [ObservableProperty]
        private Vehicle selectedVehicle;

        [ObservableProperty]
        private bool isEnableButtonCreate;
        [ObservableProperty]
        private bool isCategoryFilterVisible = false;
        [ObservableProperty]
        private bool isEventTypeFilterVisible = false;
        [ObservableProperty]
        private bool isSearchFilterVisible = false;


        [ObservableProperty]
        private string selectedEventType;
        [ObservableProperty]
        private string selectedCategory;
        [ObservableProperty]
        private string searchText;
        #endregion

        public JournalEventViewModel(IDialogService dialog, IDataService data, INavigationService navigate,IMainThreadService mainThread) : base(dialog,data,navigate)
        {
            _cts = new CancellationTokenSource();
            _mainThreadService = mainThread;
        }

        async partial void OnSelectedVehicleChanged(Vehicle value)
        {
            if(value != null)
            {
                IsEnableButtonCreate = true;
                await InitilizeJournal();

                IsEventTypeFilterVisible = true;
                IsSearchFilterVisible = true;
            }
            else IsEnableButtonCreate = false;
        }
        partial void OnSearchTextChanged(string value)
        {
            if (string.IsNullOrWhiteSpace(SearchText)) SearchJournal();
        }
        partial void OnSelectedCategoryChanged(string value)
        {
            if (!IsCategoryFilterVisible && value == null) return;
            SearchJournal();
        }
        partial void OnSelectedEventTypeChanged(string value)
        {
            if (value == EventType.Repair.GetTypeName()) IsCategoryFilterVisible = true;
            else IsCategoryFilterVisible = false;
            SearchJournal();
        }

        private async Task InitilizeJournal()
        {
            var result = await _dataService.GetFullVehicleAsync(SelectedVehicle.Id, _cts.Token);
            if (result.Success)
            {
                var resultVehicle = result as Result<Vehicle>;
                _journalDB = new List<TimelineEvent>();
                var vehicle = resultVehicle.Data;
                if (vehicle.Repairs == null || vehicle.Repairs.Count == 0) { }
                else
                {
                    foreach (var repair in resultVehicle.Data.Repairs)
                    {
                        _journalDB.Add(new TimelineEvent
                        {
                            RecordId = repair.Id,
                            Type = EventType.Repair,
                            RepairCategory = repair.RepairType.Category,
                            Date = repair.DateRepair,
                            Title = repair.RepairType.TitleRepair,
                            Cost = repair.Cost,
                            Mileage = repair.CurrentMileage + " " + vehicle.UnitDistance,
                            IconSource = repair.RepairType.Category.GetIconCategory(),
                        }); 
                    }
                }

                if (resultVehicle.Data.Refills == null || resultVehicle.Data.Refills.Count == 0) { }
                else
                {
                    foreach (var refill in resultVehicle.Data.Refills)
                    {
                        _journalDB.Add(new TimelineEvent
                        {
                            RecordId = refill.Id,
                            Type = EventType.Refill,
                            Date = refill.DateRefill,
                            Title = refill.Title,
                            Cost = refill.Cost,
                            Subtitle = refill.VolumeLiters + " " + vehicle.UnitVolume,
                            Mileage = refill.Mileage + " " + vehicle.UnitDistance,
                            IconSource = "refill_icon.png"
                        });
                    }
                }

                JournalEvents = new ObservableCollection<TimelineEvent>(_journalDB.OrderByDescending(e => e.Date));
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

                SetSearchJournal();
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
        }  // Выводит менб создания собятия для журнала

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


        public void SetSearchJournal()
        {

            foreach (EventType eventType in Enum.GetValues(typeof(EventType)))
            {
                string categoryText = eventType.GetTypeName();
                EventTypes.Add(categoryText);
            }

            foreach (RepairCategory category in Enum.GetValues(typeof(RepairCategory)))
            {
                string categoryText = category.GetDisplay();
                RepairCategories.Add(categoryText);
            }
        }
        public void SearchJournal()
        {
            // 1. Фоновая работа (Task.Run ВОЗВРАЩАЕМ!)
            Task.Run(() =>
            {

                if (_journalDB == null) return;

                var listFiltered = _journalDB.AsEnumerable();

                if (!string.IsNullOrWhiteSpace(SearchText))
                {
                    listFiltered = listFiltered.Where(c => c.Title != null && c.Title.ToLower().Contains(SearchText.ToLower()));
                }

                if (SelectedEventType != EventType.AllEvent.GetTypeName() && SelectedEventType != null)
                {
                    EventType type = SelectedEventType.GetEventType();
                    listFiltered = listFiltered.Where(c => c.Type == type);
                }

                if (SelectedCategory != RepairCategory.AllCategory.GetDisplay() && SelectedCategory != null && IsCategoryFilterVisible)
                {
                    RepairCategory typeCategory = SelectedCategory.GetCategory();
                    listFiltered = listFiltered.Where(c => c.RepairCategory == typeCategory);
                }

                var finalList = listFiltered.OrderByDescending(e => e.Date).ToList();

                // 2. Обновление UI (СТРОГО СИНХРОННО, БЕЗ ASYNC/AWAIT!)
                _mainThreadService.RunUIThread(() =>
                {
                    // Просто очищаем и заполняем текущую коллекцию.
                    // Никаких new ObservableCollection, чтобы UI не сбрасывался!
                    JournalEvents.Clear();

                    // Если список не пустой - заполняем его
                    if (finalList.Any())
                    {
                        foreach (var item in finalList)
                        {
                            JournalEvents.Add(item);
                        }
                    }
                });
            });
        }

    }
}
