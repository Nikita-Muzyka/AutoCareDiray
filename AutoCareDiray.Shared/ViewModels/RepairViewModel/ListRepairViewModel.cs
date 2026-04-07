using AutoCareDiray.Shared.Models.RepairModel;
using AutoCareDiray.Shared.Models.VehicleModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using AutoCareDiray.Shared.Interface;
using AutoCareDiray.Shared.Service.ResultService;
using System.ComponentModel;


namespace AutoCareDiray.Shared.ViewModels.RepairViewModel
{
    public partial class ListRepairViewModel : BaseViewModel
    {
        #region Основные списки классов
        public ObservableCollection<Repair> Repairs { get; set; } = new ObservableCollection<Repair>();
        public ObservableCollection<Vehicle> Vehicles { get; set; } = new ObservableCollection<Vehicle>();

        CancellationTokenSource _cts;

        [ObservableProperty]
        private Vehicle selectedVehicle;

        [ObservableProperty]
        private bool isButtonEnable = false;

        private bool _isInitialize = false;
        #endregion

        public ListRepairViewModel(IDialogService dialog, IDataService data, INavigationService navigate)
            : base(dialog, data, navigate)
        {
            _cts = new CancellationTokenSource();
        }

        [RelayCommand]
        public async Task StartLoading()
        {
            Vehicles.Clear();
            Repairs.Clear();
            var result = await _dataService.ListVehicleForListRepairAsync(_cts.Token);
            if (result.Success)
            {
                var resultVehicles = result as Result<List<Vehicle>>;
                foreach (var addcar in resultVehicles.Data)
                {
                    Vehicles.Add(addcar);
                }
                _isInitialize = true;
            }
            else
            {
                await _dialogService.ShowToastAsync(result.ErrorMessage);
            }
        } //начало загрузки страницы

        private async Task LoadRepairs()
        {
            Repairs.Clear();
            var result = await _dataService.ListRepairForVehicleAsync(SelectedVehicle.Id, _cts.Token);
            if (result.Success)
            {
                var resultRepairs = result as Result<List<Repair>>;
                foreach (var repairs in resultRepairs.Data)
                {
                    Repairs.Add(repairs);
                }
            }
        } //загрузка ремонта

        [RelayCommand]
        public async Task GoRepairCard(Repair repair)
        {
            var parametr = new Dictionary<string, object>()
            {
                ["RepairId"] = repair.Id,
            };
            await _navigationService.GoNavigation("CardRepairView", parametr);
        } //навигация перехона на карточку ремонта
        [RelayCommand]
        public async Task GoCreateRepair()
        {
            if(SelectedVehicle == null) return;
            IsButtonEnable = false;
            var parametr = new Dictionary<string, object>()
            {
                ["VehicleId"] = SelectedVehicle.Id
            };

            await _navigationService.GoNavigation("CreateRepairView",parametr);
        } //навигация создания ремонта

        [RelayCommand]
        public async void ShowRepairOptions(Repair selectedRepair)
        {
            if (selectedRepair == null) return;
            var respon = await _dialogService.ShowDisplayAction();
            if (respon == "Отмена") return;
            if (respon == "Редактировать") await UpdateRepair(selectedRepair);
            else if (respon == "Удалить") await DeleteRepair(selectedRepair);
        } //Кебабб меню вывод после нажатия

        [RelayCommand]
        public void CancelToken()
        {
            _cts.Cancel();
            _cts.Dispose();
            _cts = new CancellationTokenSource();
        } //отмена токена при закртие страницы

        [RelayCommand]
        public async Task DeleteRepair(Repair repair)
        {
            await _dataService.DeleteRepairAsync(repair, _cts.Token);
            await LoadRepairs();
        } //удаление ремонта

        [RelayCommand]
        public async Task UpdateRepair(Repair repair)
        {
            var parametr = new Dictionary<string, object>()
            {
                ["RepairId"] = repair.Id
            };

            await _navigationService.GoNavigation("CreateRepairView", parametr);
        } //редактирование ремонта

        async partial void OnSelectedVehicleChanged(Vehicle value)
        {
            IsButtonEnable = value != null;
            if(value != null) await LoadRepairs();
        } //логика при выборе авто
    }
}
