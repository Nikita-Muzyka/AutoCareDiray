using AutoCareDiray.Models;
using AutoCareDiray.Service;
using AutoCareDiray.View;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCareDiray.ViewModels
{
    public partial class UserSettingsViewModal : ObservableObject
    {
        private readonly IApiService _apiService;
        private readonly IDialogService _dialogService;

        [ObservableProperty]
        public string nickName;
        [ObservableProperty]
        public string email;
        [ObservableProperty]
        public string user_id;
        [ObservableProperty]
        public string textError;

        public UserSettingsViewModal(IApiService apiService, IDialogService dialogService) 
        {
            _apiService = apiService;
            _dialogService = dialogService;
            NickName = Preferences.Get("NickName","null");
            Email = Preferences.Get("Email", "null");
            User_id = Preferences.Get("User_id", "0");
        }

        [RelayCommand]
        public async void SaveProfil()
        {
            if(int.TryParse(User_id,out int result))
            {
                var user = new UserDTO
                {
                    NickName = NickName,
                    Email = Email,
                    User_Id = result
                };

                try
                {
                    var response = await _apiService.UpdateUserApiAsync(user);
                    TextError = response.Message;
                }
                catch (Exception ex)
                {
                    TextError = ex.ToString();
                }
            }
        }
        [RelayCommand]
        public async void ExitProfil()
        {
            var result = await _dialogService.ShowConfirmationMessage("Вы точно хотите выйти с профиля?");
            if (result == true)
            {
                Preferences.Remove("User_Id");
                Preferences.Remove("NickName");
                Preferences.Remove("Email");
                Preferences.Remove("is_login");

                await Shell.Current.GoToAsync("//AuthorizationPage");
            }
        }
        [RelayCommand]
        public async void DeleteProfil()
        {
            var result = await _dialogService.ShowConfirmationMessage("Вы точно хотите удалить пользователя?");
            if (result == true)
            {
                var UserIdString = Preferences.Get("User_id", null);
                if (int.TryParse(UserIdString, out var UserId))
                {
                    try
                    {
                        var response = await _apiService.DeleteUserApiAsync(UserId);
                        if (response.Success == true)
                        {
                            Preferences.Remove("User_Id");
                            Preferences.Remove("NickName");
                            Preferences.Remove("Email");
                            Preferences.Remove("is_login");
                        }

                        await Shell.Current.GoToAsync("//AuthorizationPage");
                    }
                    catch (Exception ex)
                    {
                        textError = ex.Message;
                    }
                }
            }
        }
    }
}
