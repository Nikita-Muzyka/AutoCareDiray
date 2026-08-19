using AutoCareDiray.Shared.ViewModels.RepairViewModel;

namespace AutoCareDiray.View.RepairView;

public partial class CardRepairView : ContentPage, IQueryAttributable
{
    private readonly CardRepairViewModel _viewModel;

    public CardRepairView(CardRepairViewModel vm)
    {
        InitializeComponent();
        _viewModel = vm;
        BindingContext = _viewModel;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        // Просто передаем весь словарь параметров во ViewModel.
        // Метод Initialize сам достанет оттуда RepairId.
        _viewModel.InitializeCommand.Execute(query);
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        // Отменяем токен загрузки при закрытии/уходе со страницы, 
        // чтобы избежать утечек памяти или крашей.
        _viewModel.CancelTokenCommand.Execute(null);
    }
}