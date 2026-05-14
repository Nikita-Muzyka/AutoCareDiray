namespace AutoCareDiray.Shared.Interface
{
    public interface IDialogService
    {
        Task ShowMessageAsync(string message); //Сообщение обычное
        Task ShowWarningLogInAsync(); //Предупреждение 
        Task ShowToastAsync(string message); // Тоаст для андроида
        Task<string> ShowDisplayAction(); //Кебаб меню
        Task<string> ShowDisplayAddMenu(); //actionSheet для отображения меню в журнале события
        Task<string> ShowDisplayAction(string title,string cancel,string text1, string text2); // ActionSheet возврат стринг
        Task<bool> ShowChoiceDisplayAlertAsync(string title, string question, string text1, string text2); // Alert возврат bool 


        //Task<bool> ShowConfirmationAsync(string vehicleName);
    }
}
