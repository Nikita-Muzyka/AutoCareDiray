using AutoCareDiray.Service;
using AutoCareDiray.ViewModels;

namespace AutoCareDiray.View;

public partial class UserSettingsPage : ContentPage
{
	public UserSettingsPage(IApiService apiService,IDialogService dialogService)
	{
		InitializeComponent();
		BindingContext = new UserSettingsViewModal(apiService,dialogService);
	}
}