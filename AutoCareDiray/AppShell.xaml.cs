using AutoCareDiray.View;
using AutoCareDiray.View.Authorization;
using AutoCareDiray.View.Maintenanse;
using AutoCareDiray.View.VehicleView;

namespace AutoCareDiray
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(RegistrationPage), typeof(RegistrationPage));
            Routing.RegisterRoute(nameof(RecoverPasswordView), typeof(RecoverPasswordView));
            Routing.RegisterRoute(nameof(CreateVehicleView), typeof(CreateVehicleView));
            Routing.RegisterRoute(nameof(CardVehicleView), typeof(CardVehicleView));
            Routing.RegisterRoute(nameof(CreateMaintenanse), typeof(CreateMaintenanse));
            Routing.RegisterRoute(nameof(ListMaintenanseView), typeof(ListMaintenanseView));
        }
    }
}
