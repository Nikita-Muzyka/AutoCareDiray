
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using System.Threading.Tasks;

namespace AutoCareDiray.Service.PopUp;

public partial class ConfirmationPopUp : Popup<bool>
{
	public ConfirmationPopUp(string message)
	{
		InitializeComponent();
        labelMessage.Text += "Вы точно хотите удалить ТС: " + message + "?";


    }

    private async void OnNoClicked(object sender, EventArgs e)
    {
        await CloseAsync(false);
    }

    private async void OnYesClicked(object sender, EventArgs e)
    {
        await CloseAsync(true);
    }
}