namespace AutoCareDiray.Shared.Interface
{
    public interface IDialogService
    {
        Task ShowMessageAsync(string message);

        Task ShowWarningLogInAsync();
        Task ShowToastAsync(string message);

        Task<bool> ShowConfirmationAsync(string vehicleName);

        Task<string> ShowDisplayAction();
    }
}
