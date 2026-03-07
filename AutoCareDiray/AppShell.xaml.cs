using AutoCareDiray.Shared.Migrations;
using AutoCareDiray.View;
using AutoCareDiray.View.Authorization;
using AutoCareDiray.View.VehicleView;
using AutoCareDiray.View.RepairView;

namespace AutoCareDiray
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            //Routing.RegisterRoute(nameof(RegistrationPage), typeof(RegistrationPage));
            //Routing.RegisterRoute(nameof(RecoverPasswordView), typeof(RecoverPasswordView));
            Routing.RegisterRoute("CreateVehicleView", typeof(CreateVehicleView));
            Routing.RegisterRoute("CardVehicleView", typeof(CardVehicleView));
            Routing.RegisterRoute("CreateRepairView", typeof(CreateRepairView));
            Routing.RegisterRoute("CardRepairView", typeof(CardRepairView));
        }
    }
}
