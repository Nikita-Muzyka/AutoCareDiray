using AutoCareDiray.Shared.ViewModels.SettingsViewModel;

namespace AutoCareDiray.View.SettingsView;

public partial class SettingsView : ContentPage
{
	SettingsViewModel _viewModel;
    public SettingsView(SettingsViewModel vm)
	{
		InitializeComponent();
		_viewModel = vm;
		BindingContext = _viewModel;
    }
}