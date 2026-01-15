using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AutoCareDiray.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using AutoCareDiray.View;
using AutoCareDiray.Service.APIResponse.UserResponse;
using AutoCareDiray.Models.Validation;
using AutoCareDiray.View.Authorization;


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
                    {;
                        StatusMessage = response.Message;
                        PreferencesSetUser(response);

                        await Shell.Current.GoToAsync("//Main/MainPage");
                    }
                    else StatusMessage = response.Message;
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
            await Shell.Current.GoToAsync(nameof(RegistrationPage));
        }

        [RelayCommand]
        public async void RecoverPassword()
        {
            await Shell.Current.GoToAsync(nameof(RecoverPasswordView));
        }

        [RelayCommand]
        public void ShowPassword()
        {
            IsPassword = !IsPassword;
            IsToggleImageButton = !IsToggleImageButton;
        }

        private void PreferencesSetUser(ApiResponse ApiResponse)
        {
            GetUserResponse? userResponse = ApiResponse as GetUserResponse;
            Preferences.Set("User_id", userResponse.User_Id.ToString());
            Preferences.Set("NickName", userResponse.NickName);
            Preferences.Set("Email", userResponse.Email);
            if(IsToggleRemember) Preferences.Set("is_login", true);
        }

       
        public void CancelToken()
        {
            _cts?.Cancel();
            _cts?.Dispose();
        }
    }
}
