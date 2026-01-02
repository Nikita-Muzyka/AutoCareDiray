using AutoCareDiray.Models;
using AutoCareDiray.Service;
using AutoCareDiray.ViewModels;
using AutoCareDiray.ViewModels.Cars;
namespace AutoCareDiray.View;

public partial class CarCardPage : ContentPage
{
	public CarCardPage(CarCardViewModal carCard)
	{
		InitializeComponent();
		BindingContext = carCard;
	}
}