using AutoCareDiray.Shared.ViewModels.RefillViewModel;
namespace AutoCareDiray.View.RefillView;

public partial class CreateRefillView : ContentPage, IQueryAttributable
{
    private readonly CreateRefillViewModel _viewModel;
    public CreateRefillView(CreateRefillViewModel viewModel)
	{
        _viewModel = viewModel;
        BindingContext = _viewModel;
        InitializeComponent();
	}

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query != null)
        {
            _viewModel.InitializeCommand.Execute(query);
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _viewModel.CancelTokenCommand.Execute(null);
        _viewModel.EventOffCommand.Execute(null);
    }
}