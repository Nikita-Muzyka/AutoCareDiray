using AutoCareDiray.Shared.ViewModels.RefillViewModel;

namespace AutoCareDiray.View.RefillView;

public partial class CardRefillView : ContentPage, IQueryAttributable
{
    private readonly CardRefillViewModel _viewModel;

    public CardRefillView(CardRefillViewModel vm)
    {
        InitializeComponent();
        _viewModel = vm;
        BindingContext = _viewModel;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        // Просто передаем весь словарь параметров во ViewModel.
        // Метод Initialize сам достанет оттуда RefillId.
        _viewModel.InitializeCommand.Execute(query);
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        // Отменяем токен загрузки при закрытии/уходе со страницы, 
        // чтобы избежать утечек памяти или зависаний приложения.
        _viewModel.CancelTokenCommand.Execute(null);
    }
}