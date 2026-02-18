using AutoCareDiray.Models.Validation;
using AutoCareDiray.Service;
using AutoCareDiray.Service.Navigation;
using AutoCareDiray.Service.Data;
using AutoCareDiray.Shared.DTOs.UserDTO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;


namespace AutoCareDiray.ViewModels.Authrozation
{
    public partial class RecoverPasswordViewModels : BaseViewModel
    {
        private RecoverValidation _recoverValidation;
        private CancellationTokenSource _cts;
        public RecoverPasswordViewModels(IApiService apiService, IDialogService dialog,IDataService dataService,INavigationService navigation,
            RecoverValidation recoverValidation) : base(apiService, dialog, dataService,navigation)
        {
            _recoverValidation = recoverValidation;
            recoverValidation.ErrorsChanged += (s,e) => OnErrorsChangedUI(e);
        }

        [ObservableProperty]
        private string login;
        [ObservableProperty]
        private string oldPassword;
        [ObservableProperty]
        private string newPassword;
        [ObservableProperty]
        private bool isOldPassword = true;
        [ObservableProperty]
        private bool isNewPassword = true;
        [ObservableProperty]
        private bool isToggleImageButton = true;
        [ObservableProperty]
        private string statusMessage;

        private bool HasErrors => _recoverValidation.HasErrors;
        private bool IsEnableChangeButton => !_recoverValidation.HasErrors;
        public string NewPasswordError => _recoverValidation.GetErrors(nameof(NewPasswordError)) as string;
        public string OldPasswordError => _recoverValidation.GetErrors(nameof(OldPasswordError)) as string;
        public string LoginErrors => _recoverValidation.GetErrors(nameof(LoginErrors)) as string;

        [RelayCommand]
        public async void RecoverPassword()
        {
            _recoverValidation.AllValidation(NewPassword, OldPassword);

            if (HasErrors) ;
            else
            {
                _cts = new CancellationTokenSource();
                var updatePassword = new UpdatePassword(-1,OldPassword, NewPassword);
                var result = await _apiService.RecoverPasswordApiAsync(Login, updatePassword, _cts.Token);

                if (result.Success)
                {
                    await _dialogService.ShowToastAsync("Пароль был обновлен");
                    Back();
                }
                else StatusMessage = result.ErrorMessage;
            }
        }
        [RelayCommand]
        public async void Back()
        {
            await Shell.Current.GoToAsync("..");
        }
        [RelayCommand]
        public void ShowPassword()
        {
            IsToggleImageButton = !IsToggleImageButton;
            IsOldPassword = !IsOldPassword;
            IsNewPassword = !IsNewPassword;
        }

        partial void OnNewPasswordChanged(string value)
        {
            _recoverValidation.ValidationNewPassword(value);
        }
        partial void OnOldPasswordChanged(string value)
        {
            _recoverValidation.ValidationOldPassword(value);
        }
        partial void OnLoginChanged(string value)
        {
            _ = DebounceSearchAsync(value);
        }

        async Task DebounceSearchAsync(string value)
        {
            try
            {
                _cts?.Cancel();
                _cts = new CancellationTokenSource();

                await Task.Delay(1000, _cts.Token);
                await _recoverValidation.ValidationLoginAsync(value, _cts.Token);
            }
            catch (TaskCanceledException) { }
        }

        private void OnErrorsChangedUI(DataErrorsChangedEventArgs e)
        {
            OnPropertyChanged(nameof(HasErrors));
            OnPropertyChanged(nameof(IsEnableChangeButton));
            OnPropertyChanged(e.PropertyName);
        }
    }
}
