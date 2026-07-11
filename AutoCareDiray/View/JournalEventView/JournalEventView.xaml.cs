using AutoCareDiray.Shared.ViewModels.JournalEventViewModel;
using CommunityToolkit.Maui.Core.Platform;
using System.Threading.Tasks;
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

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.InitilizeCommand.Execute(null);
    }
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _viewModel.CancelTokenCommand.Execute(null);
    }

    private async void SearchBar_SearchButtonPressed(object sender, EventArgs e)
    {
        await MySearch.HideKeyboardAsync(CancellationToken.None);
        _viewModel.SearchJournal();

        MySearch.Unfocus();
    }
}