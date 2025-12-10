using AutoCareDiray.Service;
using AutoCareDiray.ViewModels;

namespace AutoCareDiray.View;

public partial class CreateMaintenanse : ContentPage
{
	public CreateMaintenanse(IApiService apiService,int car_id)
	{
		InitializeComponent();
		BindingContext = new CreateMaintenanseViewModal(apiService,car_id);
	}
}