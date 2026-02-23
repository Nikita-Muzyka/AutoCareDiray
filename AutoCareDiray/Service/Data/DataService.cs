using AutoCareDiray.Shared.Data;
using AutoCareDiray.Shared.Models.RepairModel;
using AutoCareDiray.Shared.Models.VehicleModel;
using Microsoft.EntityFrameworkCore;


namespace AutoCareDiray.Service.Data
{
    public class DataService : IDataService
    {
        private readonly AppDBContex _dbContex;
        public DataService(AppDBContex db)
        {
            _dbContex = db;
        }

        //Vehicle
        public async Task CreateVehicleAsync(Vehicle vehicle, CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await _dbContex.Vehicles.AddAsync(vehicle, token);
                await _dbContex.SaveChangesAsync(token);
            }
            catch (OperationCanceledException)
            {

            }
            catch (Exception ex)
            {

            }

        }
        public async Task<List<Vehicle>> ListVehicleAsync(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                var vehicles = await _dbContex.Vehicles.ToListAsync(token);
                return vehicles;
            }
            catch (OperationCanceledException)
            {
                return new List<Vehicle>();
            }
            catch (Exception ex)
            {
                return new List<Vehicle>();
            }
        }

        public async Task<Vehicle> GetVehicleAsync(int Vehicle_Id, CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                var vehicle = await _dbContex.Vehicles.FirstOrDefaultAsync(v => v.Id == Vehicle_Id, token);
                return vehicle;
            }
            catch (OperationCanceledException)
            {
                return new Vehicle();
            }
            catch (Exception ex)
            {
                return new Vehicle();
            }
        }

        //Repair

        public async Task<List<Repair>> ListRepairForVehicleAsync(int VehicleId, CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                var repairs = await _dbContex.Repairs.Where(c => c.VehicleId == VehicleId).Include(c => c.RepairType).ToListAsync();
                return repairs;
            }
            catch (OperationCanceledException ex)
            {
                return new List<Repair>();
            }
            catch (Exception ex)
            {
                return new List<Repair>();
            }
        }

        public async Task<bool> CreateRepairAsync(Repair repair, CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await _dbContex.Repairs.AddAsync(repair, token);
                await _dbContex.SaveChangesAsync();
                return true;
            }
            catch (OperationCanceledException ex)
            {
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        //RepairType

        public async Task<List<RepairType>> GetListRepairTypeAsync(int vehicleId, CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                var repairs = await _dbContex.RepairTypes.Where(c => c.VehicleId == vehicleId).ToListAsync();
                return repairs;
            }
            catch (OperationCanceledException ex)
            {
                return new List<RepairType>();
            }
            catch (Exception ex)
            {
                return new List<RepairType>();
            }
        }

        public async Task<bool> UpdateRepairTypeAsync(RepairType repaitType, CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await _dbContex.RepairTypes.AddAsync(repaitType, token);
                await _dbContex.SaveChangesAsync();
                return true;
            }
            catch (OperationCanceledException ex)
            {
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public void InitializeDatabase()
        {
            _dbContex.Database.Migrate();
        }
    }
}
