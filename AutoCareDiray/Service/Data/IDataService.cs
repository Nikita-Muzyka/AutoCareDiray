using AutoCareDiray.Shared.Data;
using AutoCareDiray.Shared.Models.VehicleModel;
using AutoCareDiray.Shared.Models.RepairModel;
namespace AutoCareDiray.Service.Data
{
    public interface IDataService
    {

        //Vehicle
        Task CreateVehicleAsync(Vehicle vehicle,CancellationToken token);

        Task<List<Vehicle>> ListVehicleAsync(CancellationToken token);

        Task<Vehicle> GetVehicleAsync(int Vehicle_Id, CancellationToken token);

        //Repair

        Task<List<Repair>> ListRepairForVehicleAsync(int VehicleId, CancellationToken token);

        void InitializeDatabase();
    }
}
