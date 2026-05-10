using AutoCareDiray.Shared.Interface;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AutoCareDiray.Shared.Models.RepairModel;
using AutoCareDiray.Shared.Service.ResultService;

namespace AutoCareDiray.Shared.ViewModels.RepairViewModel
{
    public partial class CardRepairViewModel : BaseViewModel
    {
        #region основные классы и списки

        CancellationTokenSource _cts;
        bool _isinitilize = false;

        #endregion


        #region клссы и свойства для UI

        [ObservableProperty]
        private Repair repair;
        [ObservableProperty]
        private RepairType repairType;

        #endregion
        public CardRepairViewModel(IDialogService dialog, IDataService data, INavigationService navigate)
            : base(dialog, data, navigate)
        {
            _cts = new CancellationTokenSource();
        }

        [RelayCommand]
        public async Task Initilize(int repairId)
        {
            if (_isinitilize) return;
            var result = await _dataService.GetRepairAsync(repairId,_cts.Token);
            if (result.Success)
            {
                var resultRepair = result as Result<Repair>;
                Repair = resultRepair.Data;
                RepairType = resultRepair.Data.RepairType;
                _isinitilize = true;
            }
        }

        [RelayCommand]
        public void CancelToken()
        {
            _cts.Cancel();
            _cts.Dispose();
            _cts = new CancellationTokenSource();
        }
    }
}
