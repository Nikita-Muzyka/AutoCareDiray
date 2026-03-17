using AutoCareDiray.Shared.Interface;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AutoCareDiray.Shared.Models.RepairModel;
using AutoCareDiray.Shared.Service.ResultService;

namespace AutoCareDiray.Shared.ViewModels.RepairViewModel
{
    public partial class CardRepairViewModel : BaseViewModel
    {
        CancellationTokenSource _cts;
        bool _isinitilize = false;

        [ObservableProperty]
        public Repair repairRespon;
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
                RepairRespon = resultRepair.Data;
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
