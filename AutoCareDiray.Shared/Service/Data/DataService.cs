using AutoCareDiray.Shared.Data;
using AutoCareDiray.Shared.Interface;
using AutoCareDiray.Shared.Models.RepairModel;
using AutoCareDiray.Shared.Models.VehicleModel;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using AutoCareDiray.Shared.Service.ResultService;


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
        public async Task<Result> CreateVehicleAsync(Vehicle vehicle, CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();

                await _dbContex.Vehicles.AddAsync(vehicle, token);
                await _dbContex.SaveChangesAsync(token);
                Debug.WriteLine($"Сохранил машину в БД ид {vehicle.Id}");
                return Result.SuccessCreate();

            }
            catch (OperationCanceledException)
            {
                return Result.ErrorCreate("Операция была отменена");
            }
            catch (Exception ex)
            {
                return Result.ErrorCreate($"Произошла ошибка при сохранении машины: {ex.Message}");
            }

        }
        public async Task<Result> ListVehicleAsync(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                var vehicles = await _dbContex.Vehicles
                    .Select(c => new Vehicle { Id = c.Id, Mileage = c.Mileage, NameVehicle = c.NameVehicle })
                    .ToListAsync(token);
                if(vehicles.Count > 0) return Result<List<Vehicle>>.SuccessCreate(vehicles);
                else return Result.ErrorCreate("Список машин пуст");
            }
            catch (OperationCanceledException)
            {
                return Result.ErrorCreate("Операция была отменена");
            }
            catch (Exception ex)
            {
                return Result.ErrorCreate($"Произошла ошибка при получении списка машин: {ex.Message}");
            }
        }

        public async Task<Result> ListVehicleForListRepairAsync(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                var vehicles = await _dbContex.Vehicles
                    .Select(c => new Vehicle { Id = c.Id,NameVehicle = c.NameVehicle })
                    .ToListAsync(token);
                if(vehicles.Count > 0) return Result<List<Vehicle>>.SuccessCreate(vehicles);
                else return Result.ErrorCreate("Список машин пуст");
            }
            catch (OperationCanceledException)
            {
                return Result.ErrorCreate("Операция была отменена");
            }
            catch (Exception ex)
            {
                return Result.ErrorCreate($"Произошла ошибка при получении списка машин: {ex.Message}");
            }
        }

        public async Task<Result> GetVehicleAsync(int Vehicle_Id, CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                var vehicle = await _dbContex.Vehicles.FirstOrDefaultAsync(v => v.Id == Vehicle_Id, token);

                if (vehicle != null) return Result<Vehicle>.SuccessCreate(vehicle);
                else return Result.ErrorCreate("Машина не найдена");
            }
            catch (OperationCanceledException)
            {
                return Result.ErrorCreate("Операция была отменена");
            }
            catch (Exception ex)
            {
                return Result.ErrorCreate($"Произошла ошибка при получении машины: {ex.Message}");
            }
        }

        public async Task<Result> GetVehicleAndRepairTypesAsync(int Vehicle_Id, CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                var vehicle = await _dbContex.Vehicles
                    .Include(c => c.ReepairTypes)
                    .Select(c => new Vehicle {Id = c.Id, Mileage = c.Mileage,ReepairTypes = c.ReepairTypes})
                    .FirstOrDefaultAsync(v => v.Id == Vehicle_Id, token);
                if (vehicle != null) return Result<Vehicle>.SuccessCreate(vehicle);
                else return Result.ErrorCreate("Машина не найдена");
            }
            catch (OperationCanceledException)
            {
                return Result.ErrorCreate("Операция была отменена");
            }
            catch (Exception ex)
            {
                return Result.ErrorCreate($"Произошла ошибка при получении машины: {ex.Message}");
            }
        }

        public async Task<Result> DeleteVehicleAsync(Vehicle vehicle, CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                var respon = await _dbContex.Vehicles.FindAsync(vehicle.Id);
                if(respon != null)
                {
                    _dbContex.Vehicles.Remove(respon);
                    await _dbContex.SaveChangesAsync(token);
                    return Result.SuccessCreate();
                }
                else
                {
                    return Result.ErrorCreate("Машина не найдена");
                }
            }
            catch (OperationCanceledException)
            {
                return Result.ErrorCreate("Операция была отменена");
            }
            catch (Exception ex)
            {
                return Result.ErrorCreate($"Произошла ошибка при удалении машины: {ex.Message}");
            }
        }

        public async Task<Result> UpdateVehicleAsync(Vehicle vehicle, CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                var vehicleDb = await _dbContex.Vehicles.FindAsync(vehicle.Id, token);
                
                if(vehicleDb == null)
                {
                    return Result.ErrorCreate("Машина не найдена");
                }
                else
                {
                    vehicleDb.YearPurchase = vehicle.YearPurchase;
                    vehicleDb.YearCreate = vehicle.YearCreate;
                    vehicleDb.NameVehicle = vehicle.NameVehicle;
                    vehicleDb.VehicleType = vehicle.VehicleType;
                    vehicleDb.Mileage = vehicle.Mileage;

                    await _dbContex.SaveChangesAsync(token);

                    return Result.SuccessCreate();
                }
            }
            catch (OperationCanceledException)
            {
                return Result.ErrorCreate("Операция была отменена");
            }
            catch (Exception ex)
            {
                return Result.ErrorCreate($"Произошла ошибка при обновлении машины: {ex.Message}");
            }
        }

        public async Task<Result> UpdateVehicleMileageAsync(int vehicleId,int mileage, CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                var vehicleDB = await _dbContex.Vehicles.FindAsync(vehicleId, token);
                if (vehicleDB != null)
                {
                    vehicleDB.Mileage = mileage;
                    await _dbContex.SaveChangesAsync(token);
                    return Result.SuccessCreate();
                }
                else return Result.ErrorCreate("Машина не найдена");
            }
            catch (OperationCanceledException)
            {
                return Result.ErrorCreate("Операция была отменена");
            }
            catch (Exception ex)
            {
                return Result.ErrorCreate($"Произошла ошибка при обновлении пробега машины: {ex.Message}");
            }
        }


        //Repair

        public async Task<Result> ListRepairForVehicleAsync(int VehicleId, CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                var repairs = await _dbContex.Repairs
                    .Where(c => c.VehicleId == VehicleId)
                    .Select(c => new Repair { Id = c.Id,DateRepair = c.DateRepair,CurrentMileage = c.CurrentMileage,RepairType = c.RepairType,Vehicle = c.Vehicle })
                    .OrderBy(c => c.DateRepair)
                    .ToListAsync(token);

                if(repairs.Count > 0) return Result<List<Repair>>.SuccessCreate(repairs);
                else return Result.ErrorCreate("Список ремонтов пуст");
            }
            catch (OperationCanceledException ex)
            {
                return Result.ErrorCreate("Операция была отменена");
            }
            catch (Exception ex)
            {
                return Result.ErrorCreate($"Произошла ошибка при получении списка ремонтов: {ex.Message}");
            }
        }

        public async Task<Result> GetRepairAsync(int repairId, CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                var repairDb = await _dbContex.Repairs.FindAsync(repairId,token);

                if(repairDb != null) return Result<Repair>.SuccessCreate(repairDb);
                else return Result.ErrorCreate("Ремонт не найден");
            }
            catch (OperationCanceledException ex)
            {
                return Result.ErrorCreate("Операция была отменена");
            }
            catch (Exception ex)
            {
                return Result.ErrorCreate($"Произошла ошибка при получении ремонта: {ex.Message}"); 
            }
        }
        public async Task<Result> CreateRepairAsync(Repair repair, CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await _dbContex.Repairs.AddAsync(repair, token);
                await _dbContex.SaveChangesAsync(token);
                return Result.SuccessCreate();
            }
            catch (OperationCanceledException ex)
            {
                return Result.ErrorCreate("Операция была отменена");    
            }
            catch (Exception ex)
            {
                return Result.ErrorCreate($"Произошла ошибка при сохранении ремонта: {ex.Message}");
            }
        }

        public async Task<Result> DeleteRepairAsync(Repair repair, CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                var respon = await _dbContex.Repairs.FindAsync(repair.Id);
                if (respon != null)
                {
                    _dbContex.Repairs.Remove(respon);
                    await _dbContex.SaveChangesAsync();
                    return Result.SuccessCreate();
                }
                else return Result.ErrorCreate("Ремонт не найден");

            }
            catch (OperationCanceledException ex)
            {
                return Result.ErrorCreate("Операция была отменена");
            }
            catch (Exception ex)
            {
                return Result.ErrorCreate($"Произошла ошибка при удалении ремонта: {ex.Message}");
            }
        }

        public async Task<Result> UpdateRepairAsync(Repair repair, CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                var repairDb = await _dbContex.Repairs.FindAsync(repair.Id, token);

                if (repairDb == null) return Result.ErrorCreate("Ремонт не найден");
                else
                {
                    repairDb.SpareParts = repair.SpareParts;
                    repairDb.CurrentMileage = repair.CurrentMileage;
                    repairDb.DateRepair = repair.DateRepair;
                    repairDb.Description = repair.Description;
                    repairDb.Cost = repair.Cost;

                    await _dbContex.SaveChangesAsync(token);
                    return Result.SuccessCreate();
                }
            }
            catch (OperationCanceledException)
            {
                return Result.ErrorCreate("Операция была отменена");    
            }
            catch (Exception ex)
            {
                return Result.ErrorCreate($"Произошла ошибка при обновлении ремонта: {ex.Message}");
            }
        }


        //RepairType

        public async Task<Result> GetListRepairTypeAsync(int vehicleId, CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                var repairTypes = await _dbContex.RepairTypes.Where(c => c.VehicleId == vehicleId).ToListAsync();
                if(repairTypes != null) return Result<List<RepairType>>.SuccessCreate(repairTypes);
                else return Result.ErrorCreate("Список типов ремонтов пуст");
            }
            catch (OperationCanceledException ex)
            {
                return Result.ErrorCreate("Операция была отменена");
            }
            catch (Exception ex)
            {
                return Result.ErrorCreate($"Произошла ошибка при получении списка типов ремонтов: {ex.Message}");
            }
        }

        public async Task<Result> UpdateRepairTypeAsync(RepairType repaitType, CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                var repairDb = await _dbContex.RepairTypes.FindAsync(repaitType.Id,token);
                if (repairDb != null)
                {
                    repairDb.IntervalDate = repaitType.IntervalDate;
                    repairDb.IntervalMileagee = repaitType.IntervalMileagee;

                    await _dbContex.SaveChangesAsync();
                    return Result.SuccessCreate();
                }
                else return Result.ErrorCreate("Тип ремонта не найден");
            }
            catch (OperationCanceledException ex)
            {
                return Result.ErrorCreate("Операция была отменена");
            }
            catch (Exception ex)
            {
                return Result.ErrorCreate($"Произошла ошибка при обновлении типа ремонта: {ex.Message}");
            }
        }
        public void InitializeDatabase()
        {
            _dbContex.Database.Migrate();
        }
    }
}
