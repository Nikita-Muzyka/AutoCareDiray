using AutoCareDiray.Shared.ViewModels.RefillViewModel;
namespace AutoCareDiray.View.RefillView;

public partial class CreateRefillView : ContentPage
{
    private readonly CreateRefillViewModel _viewModel;
    public CreateRefillView(CreateRefillViewModel viewModel)
	{
        _viewModel = viewModel;
        BindingContext = _viewModel;
        InitializeComponent();
	}

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _viewModel.CancelTokenCommand.Execute(null);
    }
}