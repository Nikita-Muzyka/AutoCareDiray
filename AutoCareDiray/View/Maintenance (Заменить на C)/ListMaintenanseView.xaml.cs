using AutoCareDiray.ViewModels;
using AutoCareDiray.ViewModels.Maintenanse;
namespace AutoCareDiray.View.Maintenanse;

public partial class ListMaintenanseView : ContentPage
{
	public ListMaintenanseView(ListMaintenanseViewModel listMain)
	{
		InitializeComponent();
		BindingContext = listMain;
	}
}