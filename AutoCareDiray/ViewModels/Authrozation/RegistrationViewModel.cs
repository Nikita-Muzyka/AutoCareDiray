
using AutoCareDiray.Models.Validation;
using AutoCareDiray.Service;
using AutoCareDiray.Service.Navigation;
using AutoCareDiray.Service.Data;
using AutoCareDiray.Shared.DTOs.UserDTO;
using AutoCareDiray.Shared.Result;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;


namespace AutoCareDiray.ViewModels
{
    public partial class RegistrationViewModel : BaseViewModel
    {
        private readonly UserValidation _userValidation;
        CancellationTokenSource _cts;

        [ObservableProperty]
        private string statusMessage;
        
        [ObservableProperty]
        private string nickName;

        [ObservableProperty]
        private string? email;

        [ObservableProperty]
        private string login;
        
        [ObservableProperty]
        private string password;


        public RegistrationViewModel(IApiService apiService,IDialogService _dialogService,IDataService dataService,INavigationService navigation,
            UserValidation validation) : base(apiService, _dialogService, dataService, navigation)
        {
            _userValidation = validation;
            _cts = new CancellationTokenSource();
            _userValidation.ErrorsChanged += (s, e) => OnErrorsChangedUI(e);
        }

        public bool HasErrors => _userValidation.HasErrors;
        public bool IsValidButton => !_userValidation.HasErrors;
        public string NickNameError => _userValidation.GetErrors(nameof(NickNameError)) as string;
        public string EmailError => _userValidation.GetErrors(nameof(EmailError)) as string;
        public string LoginError => _userValidation.GetErrors(nameof(LoginError)) as string;
        public string PasswordError => _userValidation.GetErrors(nameof(PasswordError)) as string;


        [RelayCommand]
        public async Task RegistrationUserAsync()
        {
            try
            {
                _userValidation.ValidationAll(NickName, Email, Password);
                await _userValidation.ValidationLoginAsync(Login, _cts.Token);

                _cts.Token.ThrowIfCancellationRequested();

                if (!HasErrors) await RegistrationApiAsync();
            }
            catch (OperationCanceledException) { }
        }
        [RelayCommand]
        public async Task Back()
        {
            await Shell.Current.GoToAsync("..");
        }

        
        partial void OnNickNameChanged(string value)
        {
            _userValidation.ValidationNickName(value);
        }
        partial void OnEmailChanged(string value)
        {
            _userValidation.ValidationEmail(value);
        }
        partial void OnLoginChanged(string value)
        {
           _= DebounceSearchAsync(value);
        }
        partial void OnPasswordChanged(string value)
        {
            _userValidation.ValidationPassword(value);
        }
        void OnErrorsChangedUI(DataErrorsChangedEventArgs e)
        {
            OnPropertyChanged(nameof(HasErrors));
            OnPropertyChanged(nameof(IsValidButton));
            OnPropertyChanged(e.PropertyName);
            
        }

        public async Task RegistrationApiAsync()
        {
            try
            {
                UserDTO userDTO = new UserDTO(NickName, Email, Login, Password);
                var response = await _apiService.CreateUserApiAsync(userDTO, _cts.Token);
                if (response.Success == true)
                {
                    await _dialogService.ShowMessageAsync("Пользователь Создан");
                    PreferencesSetUser(response);

                    await _dialogService.ShowToastAsync("Пользователь был создан");
                    await Shell.Current.GoToAsync("..");
                }
                else StatusMessage = response.ErrorMessage;
            }
            catch (OperationCanceledException) { }
        }

         void PreferencesSetUser(Result result)
        {
            Result<UserDTO> resultUser = result as Result<UserDTO>;
            Preferences.Set("User_id", resultUser.Data.User_id.ToString());
            Preferences.Set ("NickName", resultUser.Data.NickName);
            Preferences.Set("Email", resultUser.Data.Email);
        }

        async Task DebounceSearchAsync(string value)
        {
            try
            {
                _cts?.Cancel();
                _cts = new CancellationTokenSource();

                await Task.Delay(1000, _cts.Token);
                await _userValidation.ValidationLoginAsync(value,_cts.Token);
            }
            catch (TaskCanceledException) { }
        }

        public void CancelToken()
        {
            _cts.Cancel();
            _cts.Dispose();
            _cts = new CancellationTokenSource();
        }
    }
}
