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


namespace AutoCareDiray.ViewModels
{
    
    public partial class AuthorizationViewModel : BaseViewModel
    {
        
        private CancellationTokenSource _cts;
        public AuthorizationViewModel(IApiService apiService,IDialogService dialog) :base(apiService,dialog)
        {
            
            _cts = new CancellationTokenSource();
        }

        [ObservableProperty]
        public string login;
        [ObservableProperty]
        public string password;
        [ObservableProperty]
        public string text;
        [ObservableProperty]
        public bool isToggleSwitch;
        [ObservableProperty]
        public bool isPassword = true;
        [ObservableProperty]
        public bool isTogglePasswordSwitch;
        [ObservableProperty]
        public bool isEnableLogInButton = true;

        [RelayCommand]
        public async Task LogInAsync()
        {
            try
            {
#if DEBUG 
                if (Password == "1") await Shell.Current.GoToAsync("//Main/MainPage");
#endif
                IsEnableLogInButton = false;
                bool start = LightLogInValidator.AuthValidation(Login, Password);
                if (start)
                {
                    Text = "";
                    var response = await _apiService.AuthorizationApiAsync(login, password,_cts.Token);
                    IsEnableLogInButton = true;
                    if (response.Success == true)
                    {
                        _cts.Token.ThrowIfCancellationRequested();  
                        Text = response.Message;
                        PreferencesSetUser(response);
                        
                        await Shell.Current.GoToAsync("//Main/MainPage");
                    }
                    else Text = response.Message;
                }
                else Text = "Пароль и Логин не могут быть пустыми";
            }
            catch(OperationCanceledException) { IsEnableLogInButton = true; }
            catch (Exception ex) { }
        }
        [RelayCommand]
        public async Task RegistrationAsync()
        {
            await Shell.Current.GoToAsync(nameof(RegistrationPage));
        }

        void PreferencesSetUser(ApiResponse ApiResponse)
        {
            GetUserResponse? userResponse = ApiResponse as GetUserResponse;
            Preferences.Set("User_id", userResponse.User_Id.ToString());
            Preferences.Set("NickName", userResponse.NickName);
            Preferences.Set("Email", userResponse.Email);
            if(IsToggleSwitch) Preferences.Set("is_login", true);
        }

        partial void OnIsTogglePasswordSwitchChanged(bool value)
        {
            IsPassword = !IsTogglePasswordSwitch;
        }

        public void CancelToken()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = new CancellationTokenSource();
        }
    }
}
