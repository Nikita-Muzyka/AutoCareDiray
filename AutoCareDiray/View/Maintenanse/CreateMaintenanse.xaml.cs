using AutoCareDiray.Service;
using AutoCareDiray.ViewModels;

namespace AutoCareDiray.View;

public partial class CreateMaintenanse : ContentPage
{
	public CreateMaintenanse(CreateMaintenanseViewModal createMain)
	{
		InitializeComponent();
		BindingContext = createMain;
	}
}