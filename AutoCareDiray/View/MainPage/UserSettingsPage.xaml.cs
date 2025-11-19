using AutoCareDiray.Service;
using AutoCareDiray.ViewModels;

namespace AutoCareDiray.View;

public partial class UserSettingsPage : ContentPage
{
	public UserSettingsPage(IApiService apiService)
	{
		InitializeComponent();
		BindingContext = new UserSettingsViewModal(apiService);
	}
}