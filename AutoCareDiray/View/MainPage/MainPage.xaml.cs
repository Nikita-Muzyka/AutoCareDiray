using AutoCareDiray.ViewModels;

namespace AutoCareDiray.View;

public partial class MainPage : ContentPage
{
	public MainPage(MainPageViewModal mainVM)
	{
		InitializeComponent();
		BindingContext = mainVM;
	}
}