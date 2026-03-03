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
            Routing.RegisterRoute("createVehicle", typeof(CreateVehicleView));
            Routing.RegisterRoute("cardVehicle", typeof(CardVehicleView));
            Routing.RegisterRoute("createRepair", typeof(CreateRepairView));
        }
    }
}
