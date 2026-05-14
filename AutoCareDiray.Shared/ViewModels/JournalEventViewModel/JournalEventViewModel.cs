using AutoCareDiray.Shared.Interface;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace AutoCareDiray.Shared.ViewModels.JournalEventViewModel
{
    public partial class JournalEventViewModel : BaseViewModel
    {
        private CancellationTokenSource _cts;

        [ObservableProperty]
        private string text = "dawdaw";
        public JournalEventViewModel(IDialogService dialog, IDataService data, INavigationService navigate) : base(dialog,data,navigate)
        {
            _cts = new CancellationTokenSource();
        }

        [RelayCommand]
        public async Task ShowMenu()
        {
            var result = await _dialogService.ShowDisplayAddMenu();

            switch (result)
            {
                case "⛽ Заправку":
                    await _navigationService.GoNavigation("CreateRefillView");
                    break;

                case "🛠 Ремонт":
                    await _navigationService.GoNavigation("CreateRepairView");
                    break;

                case "🧾 Прочий расход":
                    // Переходим на страницу расхода (мойка, страховка)
                    break;
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
