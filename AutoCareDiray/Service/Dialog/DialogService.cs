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

        public async Task ShowWarningLogInAsync()
        {
            var popup = new InformationPopUp();
            await Application.Current.MainPage.ShowPopupAsync(popup, new PopupOptions
            {
                PageOverlayColor = Colors.Transparent.WithAlpha(0.0f),
                Shape = null,
                CanBeDismissedByTappingOutsideOfPopup = false
            });

        }

        public async Task ShowToastAsync(string message)
        {
            if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
            {
                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

                ToastDuration duration = ToastDuration.Short;
                double fontSize = 14;

                var toast = Toast.Make(message, duration, fontSize);

                await toast.Show(cancellationTokenSource.Token);
            }
        }
    }
}
