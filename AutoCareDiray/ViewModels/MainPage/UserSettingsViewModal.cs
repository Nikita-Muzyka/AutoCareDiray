using AutoCareDiray.Models;
using AutoCareDiray.Service;
using AutoCareDiray.View;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AutoCareDiray.ViewModels
{
    public partial class UserSettingsViewModal : BaseViewModel
    {

        private CancellationTokenSource _cts;

        [ObservableProperty]
        public string nickName;
        [ObservableProperty]
        public string email;
        [ObservableProperty]
        public string user_id;
        [ObservableProperty]
        public string textError;

        public UserSettingsViewModal(IApiService apiService, IDialogService dialogService) :base(apiService, dialogService)
        {
            _cts = new CancellationTokenSource();
        }

        [RelayCommand]
        public void LoadUserData()
        {
            NickName = Preferences.Get("NickName", "null");
            Email = Preferences.Get("Email", "null");
            User_id = Preferences.Get("User_id", "0");
        }
        [RelayCommand]
        public async Task SaveProfil()
        {
            if(int.TryParse(User_id,out int result))
            {
                try
                {
                    var userRequest = new UserUpdateRequest(result, NickName, Email, "Null");
                    _cts.Token.ThrowIfCancellationRequested();
                    var response = await _apiService.UpdateUserApiAsync(userRequest, _cts.Token);
                    await _dialogService.ShowMessage(response.Message);
                }
                catch (OperationCanceledException) { }
            }
        }
        [RelayCommand]
        public async Task ExitProfil()
        {
            try
            {
                var result = await _dialogService.ShowConfirmationMessage("Вы точно хотите выйти с профиля?");
                _cts.Token.ThrowIfCancellationRequested();
                if (result == true)
                {
                    Preferences.Remove("User_Id");
                    Preferences.Remove("NickName");
                    Preferences.Remove("Email");
                    Preferences.Remove("is_login");

                    await Shell.Current.GoToAsync("//AuthorizationPage");
                }
            }
            catch (OperationCanceledException) { }
        }
        [RelayCommand]
        public async Task DeleteProfil()
        {
            try
            {
                var result = await _dialogService.ShowConfirmationMessage("Вы точно хотите удалить пользователя?");
                _cts.Token.ThrowIfCancellationRequested();
                if (result == true)
                {
                    var UserIdString = Preferences.Get("User_id", null);
                    if (int.TryParse(UserIdString, out var UserId))
                    {
                        var response = await _apiService.DeleteUserApiAsync(UserId, _cts.Token);
                        if (response.Success == true)
                        {
                            Preferences.Remove("User_Id");
                            Preferences.Remove("NickName");
                            Preferences.Remove("Email");
                            Preferences.Remove("is_login");

                            await Shell.Current.GoToAsync("///AuthorizationPage");
                        }
                        else await _dialogService.ShowMessage(response.Message);
                    }
                }
            }
            catch (OperationCanceledException) { }
        }

        public void CancelToken()
        {
                _cts.Cancel();
                _cts.Dispose();
                _cts = new CancellationTokenSource();
        }
    }
}
