using AutoCareDiray.Service.PopUp;
using AutoCareDiray.Shared.Interface;
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

namespace AutoCareDiray.Service.Dialog;

class DialogService : IDialogService
{
    public async Task ShowMessageAsync(string message)
    {
        await Shell.Current.DisplayAlert("",message,"Ok");
    } //Сообщение обычное

    public async Task ShowWarningLogInAsync()
    {
        var popup = new InformationPopUp();
        await Application.Current.MainPage.ShowPopupAsync(popup, new PopupOptions
        {
            PageOverlayColor = Colors.Transparent.WithAlpha(0.0f),
            Shape = null,
            CanBeDismissedByTappingOutsideOfPopup = false
        });

    } //Предупреждение 

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
    } // Тоаст для андроида

    public async Task<string> ShowDisplayAction()
    {
        string response = await Application.Current.MainPage.DisplayActionSheet("Выберите действие", "Отмена", null, "Редактировать", "Удалить");
        return response;
    } //Кебаб меню

    public async Task<string> ShowDisplayAction(string title, string cancel, string text1, string text2)
    {
        string response = await Application.Current.MainPage.DisplayActionSheet(title, cancel, null, text1, text2);
        return response;
    }  // ActionSheet возврат стринг

    public async Task<string> ShowDisplayAddMenu()
    {
        string action = await Shell.Current.DisplayActionSheet(
            "Что хотите добавить?",
            null,
            null,
            "⛽ Заправку",
            "🛠 Ремонт",
            "🧾 Сарховка,счета и тд",
            "🧾 Прочий расход");

        return action;
    }  //actionSheet для отображения меню в журнале события
    public async Task<bool> ShowChoiceDisplayAlertAsync(string title, string question, string text1, string text2)
    {
        var answer = await Shell.Current.DisplayAlert(title, question, text1, text2);
        return answer;
    } // Alert возврат bool 

    //public async Task<bool> ShowConfirmationAsync(string vehicleName)
    //{
    //    var popup = new ConfirmationPopUp(vehicleName);

    //    var result = await Application.Current.MainPage.ShowPopupAsync<bool>(popup, new PopupOptions
    //    {
    //        PageOverlayColor = Colors.Transparent.WithAlpha(0.0f),
    //        Shape = null,
    //        CanBeDismissedByTappingOutsideOfPopup = false
    //    });

    //    if (result.Result == true) return true;
    //    else return false;
    //} 
}

