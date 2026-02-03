using AutoCareDiray.Models.PopUp;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Extensions;
using Microsoft.Maui.Controls.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCareDiray.Service
{
    class DialogService : IDialogService
    {
        public async Task<bool> ShowConfirmationMessage(string message)
        {
            var popup = new ConfirmationPopup(message);
            await Application.Current.MainPage.ShowPopupAsync(popup);

            bool result = await popup.Result;

            return result;
        }
        public async Task ShowMessageAsync(string message)
        {
            await Shell.Current.DisplayAlert("",message,"Ok");
        }

        public async Task ShowInfoAsync()
        {
            var popup = new InformationPopUp("Предупреждение!","1. Без регистрации вы не сможете переносить данные на другой телефон или планшет ",
                "2. Вы не сможете сохранять данные на сервере только на памяти телефона, что занимает память телефона");
            await Application.Current.MainPage.ShowPopupAsync(popup, new PopupOptions
            {
                PageOverlayColor = Colors.Transparent.WithAlpha(0.0f),
                Shape = null,
                CanBeDismissedByTappingOutsideOfPopup = false
            });

        }

        public async Task ShowToastAsync(string message)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            ToastDuration duration = ToastDuration.Short;
            double fontSize = 14;

            var toast = Toast.Make(message, duration, fontSize);

            await toast.Show(cancellationTokenSource.Token);
        }
    }
}
