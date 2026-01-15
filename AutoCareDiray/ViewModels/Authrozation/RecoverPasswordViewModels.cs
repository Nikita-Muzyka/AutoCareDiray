using AutoCareDiray.Models.Validation;
using AutoCareDiray.Service;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoCareDiray.Models;

namespace AutoCareDiray.ViewModels.Authrozation
{
    public partial class RecoverPasswordViewModels : BaseViewModel
    {
        private UserValidation _userValidation;
        private CancellationTokenSource _cts;
        public RecoverPasswordViewModels(IApiService apiService, IDialogService dialog,UserValidation validation) : base(apiService, dialog)
        {
            _userValidation = validation;
            _userValidation.ErrorsChanged += (s,e) => OnErrorsChangedUI(e);
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
        private bool isEnableChangeButton = true;

        private string propertyPassword => _userValidation.propertyPassword;

        private bool HasErrors => _userValidation.HasErrors;
        public string PasswordErrors => _userValidation.GetErrors(propertyPassword) as string;

        [RelayCommand]
        public async void ChangePassword()
        {
            _userValidation.ValidationPassword(NewPassword);
            if (HasErrors) IsEnableChangeButton = false;
            else
            {
                _cts = new CancellationTokenSource();
                var updatePassword = new UpdateUserPassword(Login,OldPassword,NewPassword);
                var result = await _apiService.UpdatePasswordApiAsync(updatePassword,_cts.Token);

                if (result.Success)
                {
                    await _dialogService.ShowMessage(result.Message);
                    Back();
                }
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
            _userValidation.ValidationPassword(value);
        }

        private void OnErrorsChangedUI(DataErrorsChangedEventArgs e)
        {
            OnPropertyChanged(nameof(HasErrors));
            OnPropertyChanged(nameof(PasswordErrors));
        }
    }
}
