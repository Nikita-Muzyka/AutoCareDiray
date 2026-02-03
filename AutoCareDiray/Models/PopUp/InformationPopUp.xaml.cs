using CommunityToolkit.Maui.Views;

namespace AutoCareDiray.Models.PopUp;

public partial class InformationPopUp : Popup
{
    public InformationPopUp(string messageStatus,string message,string messageTwo)
    {
        InitializeComponent();
        MessageStatus.Text = messageStatus;
        MessageText.Text = message;
        MessageTextTwo.Text = messageTwo;
    }

    private void OnCloseClicked(object sender, EventArgs e)
    {
        CloseAsync();
    }


}