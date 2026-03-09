using AutoCareDiray.Shared.Interface;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AutoCareDiray.Shared.Models.RepairModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            RepairRespon = await _dataService.GetRepairAsync(repairId,_cts.Token);
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
