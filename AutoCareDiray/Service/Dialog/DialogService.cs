using CommunityToolkit.Maui.Extensions;
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
    }
}
