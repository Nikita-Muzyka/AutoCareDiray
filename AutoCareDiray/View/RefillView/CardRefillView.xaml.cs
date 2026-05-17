

using AutoCareDiray.Shared.ViewModels.RefillViewModel;
using AutoCareDiray.Shared.ViewModels.RepairViewModel;

namespace AutoCareDiray.View.RefillView;

public partial class CardRefillView : ContentPage, IQueryAttributable
{
    CardRefillViewModel _viewModel;
    public CardRefillView(CardRefillViewModel vm)
	{
		InitializeComponent();
        _viewModel = vm;
        BindingContext = _viewModel;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> parametr)
    {
        if (parametr.TryGetValue("RefillId", out var obj))
        {
            if (obj is int result)
            {
                _viewModel.InitilizeCommand.Execute(result);
            }
        }

    }
}