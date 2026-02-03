using CommunityToolkit.Maui.Views;

namespace AutoCareDiray.Models.PopUp;

public partial class InformationPopUp : Popup
{
    public InformationPopUp(string message)
    {
        InitializeComponent();
        MessageLabel.Text = message; 
    }

    private void OnCloseClicked(object sender, EventArgs e)
    {
        CloseAsync();
    }


}