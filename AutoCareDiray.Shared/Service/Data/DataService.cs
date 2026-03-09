using AutoCareDiray.Shared.Data;
using AutoCareDiray.Shared.Interface;
using AutoCareDiray.Shared.Models.RepairModel;
using AutoCareDiray.Shared.Models.VehicleModel;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;


namespace AutoCareDiray.Shared.Service.Data
{
    public class DataService : IDataService
    {
        private readonly AppDBContex _dbContex;
        public DataService(AppDBContex db)
        {
            _dbContex = db;
        }

        //Vehicle
        public async Task<bool> CreateVehicleAsync(Vehicle vehicle, CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();

                await _dbContex.Vehicles.AddAsync(vehicle, token);
                await _dbContex.SaveChangesAsync(token);
                Debug.WriteLine($"Сохранил машину в БД ид {vehicle.Id}");
                return true;    

            }
            catch (OperationCanceledException)
            {
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        public async Task<List<Vehicle>> ListVehicleAsync(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                var vehicles = await _dbContex.Vehicles.AsNoTracking().ToListAsync(token);
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

        public async Task<Vehicle> GetVehicleAndRepairTypesAsync(int Vehicle_Id, CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                var vehicle = await _dbContex.Vehicles
                    .Include(c => c.ReepairTypes)
                    .FirstOrDefaultAsync(v => v.Id == Vehicle_Id, token);
                return vehicle ?? new Vehicle();
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

        public async Task DeleteVehicleAsync(Vehicle vehicle, CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                _dbContex.Vehicles.Remove(vehicle);
                await _dbContex.SaveChangesAsync(token);
            }
            catch (OperationCanceledException)
            {

            }
            catch (Exception ex)
            {

            }
        }

        public async Task<bool> UpdateVehicleAsync(Vehicle vehicle, CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                var vehicleDb = await _dbContex.Vehicles.FindAsync(vehicle.Id, token);
                
                if(vehicleDb == null)
                {
                    return false;
                }
                else
                {
                    vehicleDb.YearPurchase = vehicle.YearPurchase;
                    vehicleDb.YearCreate = vehicle.YearCreate;
                    vehicleDb.NameVehicle = vehicle.NameVehicle;
                    vehicleDb.VehicleType = vehicle.VehicleType;

                    await _dbContex.SaveChangesAsync(token);

                    return true;
                }
            }
            catch (OperationCanceledException)
            {
                return false;
            }
            catch (Exception ex)
            {
                return false;
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

        public async Task<Repair> GetRepairAsync(int repairId, CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                var repairDb = await _dbContex.Repairs.FindAsync(repairId);
                return repairDb ?? new Repair();
            }
            catch (OperationCanceledException ex)
            {
                return new Repair();
            }
            catch (Exception ex)
            {
                return new Repair();
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

        public async Task DeleteRepairAsync(Repair repair, CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                _dbContex.Repairs.Remove(repair);
                await _dbContex.SaveChangesAsync();

            }
            catch (OperationCanceledException ex)
            {

            }
            catch (Exception ex)
            {

            }
        }

        public async Task<bool> UpdateRepairAsync(Repair repair, CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                var repairDb = await _dbContex.Repairs.FindAsync(repair.Id, token);

                if (repairDb == null)
                {
                    return false;
                }
                else
                {
                    repairDb.SpareParts = repair.SpareParts;
                    repairDb.CurrentMileage = repair.CurrentMileage;
                    repairDb.DateRepair = repair.DateRepair;
                    repairDb.Description = repair.Description;
                    repairDb.Cost = repair.Cost;

                    await _dbContex.SaveChangesAsync(token);

                    return true;
                }
            }
            catch (OperationCanceledException)
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
                var repairDb = await _dbContex.RepairTypes.FindAsync(repaitType.Id,token);
                if (repairDb != null)
                {
                    repairDb.IntervalDate = repaitType.IntervalDate;
                    repairDb.IntervalMileagee = repaitType.IntervalMileagee;
                }
                else return false;

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
