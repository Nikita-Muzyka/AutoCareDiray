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

        //Task<Vehicle> GetVehicleAndRepairTypeAsync(int Vehicle_Id, CancellationToken token);

        //Repair

        Task<List<Repair>> ListRepairForVehicleAsync(int VehicleId, CancellationToken token);

        Task<bool> CreateRepairAsync(Repair repair, CancellationToken token);


        //RepairType
        Task<List<RepairType>> GetListRepairTypeAsync(int vehicleId,CancellationToken token);

        Task<bool> UpdateRepairTypeAsync(RepairType repaitType, CancellationToken token);

        void InitializeDatabase();
    }
}
