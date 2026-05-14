
using AutoCareDiray.View;
using AutoCareDiray.View.VehicleView;
using AutoCareDiray.View.RepairView;
using AutoCareDiray.View.RefillView;

namespace AutoCareDiray
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("CreateRefillView", typeof(CreateRefillView));
            Routing.RegisterRoute("CreateVehicleView", typeof(CreateVehicleView));
            Routing.RegisterRoute("CardVehicleView", typeof(CardVehicleView));
            Routing.RegisterRoute("CreateRepairView", typeof(CreateRepairView));
            Routing.RegisterRoute("CardRepairView", typeof(CardRepairView));
        }
    }
}
