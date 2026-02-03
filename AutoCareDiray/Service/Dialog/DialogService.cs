using AutoCareDiray.Models.PopUp;
using CommunityToolkit.Maui;
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
        public async Task ShowMessage(string message)
        {
            await Shell.Current.DisplayAlert("",message,"Ok");
        }

        public async Task ShowInfo(string message)
        {
            var popup = new InformationPopUp(message);
            await Application.Current.MainPage.ShowPopupAsync(popup, new PopupOptions
            {
                PageOverlayColor = Colors.Transparent.WithAlpha(0.0f),
                Shape = null
            });

        }
    }
}
