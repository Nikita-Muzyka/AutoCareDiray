using AutoCareDiray.Shared.Models.VehicleModel;
using AutoCareDiray.Shared.Models.RepairModel;

namespace AutoCareDiray.Shared.Interface
{
    public interface IDataService
    {

        //Vehicle
        Task<bool> CreateVehicleAsync(Vehicle vehicle,CancellationToken token);

        Task<List<Vehicle>> ListVehicleAsync(CancellationToken token);

        Task<Vehicle> GetVehicleAsync(int Vehicle_Id, CancellationToken token);

        Task<Vehicle> GetVehicleAndRepairTypesAsync(int Vehicle_Id, CancellationToken token);

        Task DeleteVehicleAsync(Vehicle vehicle, CancellationToken token);

        Task<bool> UpdateVehicleAsync(Vehicle vehicle, CancellationToken token);

        //Repair

        Task<List<Repair>> ListRepairForVehicleAsync(int VehicleId, CancellationToken token);

        Task<Repair> GetRepairAsync(int repairId, CancellationToken token);

        Task<bool> CreateRepairAsync(Repair repair, CancellationToken token);

        Task DeleteRepairAsync(Repair repair, CancellationToken token);

        Task<bool> UpdateRepairAsync(Repair repair, CancellationToken token);

        //RepairType
        Task<List<RepairType>> GetListRepairTypeAsync(int vehicleId,CancellationToken token);

        Task<bool> UpdateRepairTypeAsync(RepairType repaitType, CancellationToken token);

        void InitializeDatabase();
    }
}
