using AutoCareDiray.Shared.ViewModels.JournalEventViewModel;
namespace AutoCareDiray.View.JournalEventView;

public partial class JournalEventView : ContentPage
{
	JournalEventViewModel _viewModel;
    public JournalEventView(JournalEventViewModel vm)

    {
		_viewModel = vm;
		InitializeComponent();
        BindingContext = _viewModel;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _viewModel.CancelTokenCommand.Execute(null);
    }
}