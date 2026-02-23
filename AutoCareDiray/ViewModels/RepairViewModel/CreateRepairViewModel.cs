using AutoCareDiray.Service;
using AutoCareDiray.Service.Data;
using AutoCareDiray.Service.Navigation;
using AutoCareDiray.Shared.Models.RepairModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace AutoCareDiray.ViewModels.RepairViewModel
{
    [QueryProperty(nameof(VehicleIdGet),"VehicleId")]
    public partial class CreateRepairViewModel : BaseViewModel
    {
        CancellationTokenSource _cts;
        private int VehicleIdGet { get; set; }
        public ObservableCollection<RepairType> RepairTypes { get; set; } = new ObservableCollection<RepairType>();

        [ObservableProperty]
        private RepairType selectedRepairType;
        [ObservableProperty]
        private int intervalMileageFilled;
        [ObservableProperty]
        private DateTime dateRepairSelected;
        [ObservableProperty]
        private DateTime intervalDateSelected;
        [ObservableProperty]
        private int mileageFilled;
        [ObservableProperty]
        private string sparePartsFilled;
        [ObservableProperty]
        private int costFilled;
        [ObservableProperty]
        private string descriptionFilled;

        [ObservableProperty]
        private string statusMessage;
        public CreateRepairViewModel(IApiService api,IDialogService dialog,IDataService data,INavigationService navigate) 
            : base(api, dialog, data, navigate)
        {
            _cts = new CancellationTokenSource();
        }

        [RelayCommand]
        public async void Loading()
        {
            foreach (var repairs in await _dataService.GetListRepairTypeAsync(VehicleIdGet, _cts.Token))
            {
                if (repairs is not null) RepairTypes.Add(repairs);
            }
        }

        [RelayCommand]
        public async void CreateRepair()
        {
            Repair repairCreate = new Repair()
            {
                DateRepair = DateOnly.FromDateTime(DateRepairSelected),
                SpareParts = SparePartsFilled,
                Cost = CostFilled,
                Description = DescriptionFilled,
                VehicleId = VehicleIdGet,
                RepairTypeId = SelectedRepairType.Id
            };

            var repairSuccess = await _dataService.CreateRepairAsync(repairCreate,_cts.Token);

            SelectedRepairType.IntervalDate = DateOnly.FromDateTime(IntervalDateSelected);
            SelectedRepairType.IntervalMileagee = IntervalMileageFilled;

            var repairTypeSucces = await _dataService.UpdateRepairTypeAsync(SelectedRepairType, _cts.Token);

            if(repairSuccess && repairTypeSucces)
            {
                await _navigationService.GoToBack();
            }
            else
            {
                StatusMessage = "Ремонт или интервалы не были сохранены";
            }
        }




    }
}
