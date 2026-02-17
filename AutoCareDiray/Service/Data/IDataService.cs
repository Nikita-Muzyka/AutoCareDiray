using AutoCareDiray.Shared.Data;
using AutoCareDiray.Shared.Models.VehicleModel;

namespace AutoCareDiray.Service.Data
{
    public interface IDataService
    {
        Task CreateVehicleAsync(Vehicle vehicle,CancellationToken token);

        Task<IEnumerable<Vehicle>> ListVehicleAsync(CancellationToken token);

        Task<Vehicle> GetVehicleAsync(int Vehicle_Id, CancellationToken token);

        void InitializeDatabase();
    }
}
