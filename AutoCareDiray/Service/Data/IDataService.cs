using AutoCareDiray.Shared.Data;
using AutoCareDiray.Shared.Models.VehicleModel;

namespace AutoCareDiray.Service.Data
{
    public interface IDataService
    {
        Task CreateVehicleAsync(Vehicle vehicle);

        Task<IEnumerable<Vehicle>> ListVehicleAsync();

        void InitializeDatabase();
    }
}
