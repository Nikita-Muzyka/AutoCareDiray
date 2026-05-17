using AutoCareDiray.Shared.Interface;
using CommunityToolkit.Mvvm.Input;
using AutoCareDiray.Shared.Service.ResultService;
using AutoCareDiray.Shared.Models.RefillModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AutoCareDiray.Shared.ViewModels.RefillViewModel
{
    public partial class CardRefillViewModel : BaseViewModel
    {
        #region основные классы

        CancellationTokenSource _cts;

        #endregion

        #region основные классы

        [ObservableProperty]
        private Refill refillCard;

        #endregion

        public CardRefillViewModel(IDialogService dialog, IDataService data, INavigationService navigate) : base(dialog, data, navigate)
        {
            _cts = new CancellationTokenSource();
        }

        [RelayCommand]
        public async Task Initilize(int id)
        {
          var result = await _dataService.GetRefillAsync(id, _cts.Token);
            if (result.Success)
            {
                var resultRefill = result as Result<Refill>;
                RefillCard = resultRefill.Data;
            }
        }

        [RelayCommand]
        public void CancelToken()
        {
            _cts.Cancel();
            _cts.Dispose();
            _cts = new CancellationTokenSource();
        } //отмена токена при закртие страницы
    }
}
