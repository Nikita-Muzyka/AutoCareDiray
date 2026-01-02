using AutoCareDiray.View;
using AutoCareDiray.View.Maintenanse;

namespace AutoCareDiray
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(RegistrationPage), typeof(RegistrationPage));
            Routing.RegisterRoute(nameof(CreateCarsPage), typeof(CreateCarsPage));
            Routing.RegisterRoute(nameof(CarCardPage), typeof(CarCardPage));
            Routing.RegisterRoute(nameof(CreateMaintenanse), typeof(CreateMaintenanse));
            Routing.RegisterRoute(nameof(ListMaintenanseView), typeof(ListMaintenanseView));
        }
    }
}
