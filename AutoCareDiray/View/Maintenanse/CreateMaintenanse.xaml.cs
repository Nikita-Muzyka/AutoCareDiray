using AutoCareDiray.Service;
using AutoCareDiray.ViewModels;

namespace AutoCareDiray.View;

public partial class CreateMaintenanse : ContentPage
{
	public CreateMaintenanse(IApiService apiService)
	{
		InitializeComponent();
		BindingContext = new CreateMaintenanseViewModal(apiService);
	}
}