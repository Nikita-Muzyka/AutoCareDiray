using AutoCareDiray.Shared.Interface;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCareDiray.Shared.ViewModels.RefillViewModel
{
    public partial class CreateRefillViewModel : BaseViewModel
    {
        #region основные классы

        private CancellationTokenSource _cts;
        private bool _isInitilize = false;
        private bool _isUpdate = false;

        #endregion

        #region для работы UI

        [ObservableProperty]
        private string buttonName = "Добавить";

        #endregion
        public CreateRefillViewModel(IDialogService dialog, IDataService data, INavigationService navigate) :base(dialog,data,navigate)
        {
            _cts = new CancellationTokenSource();
        }

        [RelayCommand]
        public void ProcessingRefillCommand()
        {

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
