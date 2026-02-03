using AutoCareDiray.Models.Validation;
using AutoCareDiray.Service;
using AutoCareDiray.Shared.DTOs.UserDTO;
using AutoCareDiray.Shared.Result;
using AutoCareDiray.View;
using AutoCareDiray.View.Authorization;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Devices;


namespace AutoCareDiray.ViewModels
{
    
    public partial class AuthorizationViewModel : BaseViewModel
    {
        
        private CancellationTokenSource _cts;
        public AuthorizationViewModel(IApiService apiService,IDialogService dialog) :base(apiService,dialog)
        {
        }

        [ObservableProperty]
        private string login;
        [ObservableProperty]
        private string password;
        [ObservableProperty]
        private string statusMessage = String.Empty;
        [ObservableProperty]
        private bool isToggleRemember;
        [ObservableProperty]
        private bool isPassword = true;
        [ObservableProperty]
        private bool isLoginButtonEnable = true;
        [ObservableProperty]
        private bool isToggleImageButton = true;

        [RelayCommand]
        public async Task LogInAsync()
        {
            try
            {
#if DEBUG 
                Password.Trim();
                Login.Trim();
                if (Password == "1") await Shell.Current.GoToAsync("//Main/MainPage");
#endif
                _cts = new CancellationTokenSource();
                IsLoginButtonEnable = false;

                if (LightLogInValidator.AuthValidation(Login, Password))
                {
                    var response = await _apiService.AuthorizationApiAsync(Login, Password, _cts.Token);
                    IsLoginButtonEnable = true;

                    if (response.Success == true)
                    {
                        PreferencesSetUser(response);

                        await Shell.Current.GoToAsync("//Main/MainPage");
                    }
                    else StatusMessage = response.ErrorMessage;
                }
                else
                {
                    IsLoginButtonEnable = true;
                    StatusMessage = "Пароль и Логин не могут быть пустыми";
                }
            }
            catch(OperationCanceledException) { IsLoginButtonEnable = true; }
            catch (Exception ex) { StatusMessage = ex.Message; }
        }

        [RelayCommand]
        public async Task RegistrationAsync()
        {
            await _dialogService.ShowToastAsync("Пользователь был создан");
            await Shell.Current.GoToAsync(nameof(RegistrationPage));
        }

        [RelayCommand]
        public async void RecoverPassword()
        {
            await Shell.Current.GoToAsync(nameof(RecoverPasswordView));
        }

        [RelayCommand]
        public async void LogInWithout()
        {
            Preferences.Clear();
            Preferences.Set("LoginWithout", true);
            Preferences.Set("is_login", true);
            await Shell.Current.GoToAsync("//Main/MainPage");
        }

        [RelayCommand]
        public void ShowPassword()
        {
            IsPassword = !IsPassword;
            IsToggleImageButton = !IsToggleImageButton;
        }

        private void PreferencesSetUser(Result result)
        {
            Result<UserDTO> resultUser = result as Result<UserDTO>;
            Preferences.Set("User_id", resultUser.Data.User_id.ToString());
            Preferences.Set("NickName", resultUser.Data.NickName);
            Preferences.Set("Email", resultUser.Data.Email);
            if(IsToggleRemember) Preferences.Set("is_login", true);
        }

       
        public void CancelToken()
        {
            _cts?.Cancel();
            _cts?.Dispose();
        }
    }
}
