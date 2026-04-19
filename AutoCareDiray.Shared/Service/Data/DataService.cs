using AutoCareDiray.Shared.Data;
using AutoCareDiray.Shared.Interface;
using AutoCareDiray.Shared.Models.RepairModel;
using AutoCareDiray.Shared.Models.VehicleModel;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using AutoCareDiray.Shared.Service.ResultService;
using AutoCareDiray.Shared.Models.Notes;


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

        } // Создание авто
        public async Task<Result> ListVehicleAsync(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                var vehicles = await _dbContex.Vehicles
                    .Include(c => c.RepairTypes)
                    .Select(c => new Vehicle { Id = c.Id, Mileage = c.Mileage, NameVehicle = c.NameVehicle,RepairTypes = c.RepairTypes })
                    .OrderBy(c => c.Mileage)
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
        } // список авто вместе с типоми ремонта
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
        } // список авто только название и ид
        public async Task<Result> GetVehicleAsync(int Vehicle_Id, CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                var vehicle = await _dbContex.Vehicles
                    .Include(c=> c.RepairTypes)
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
        } // получение авто
        public async Task<Result> GetVehicleMileageAsync(int Vehicle_Id, CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                var vehicle = await _dbContex.Vehicles
                    .Select(c => new Vehicle
                    {
                        Id = Vehicle_Id,
                        Mileage = c.Mileage,
                    })
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
        } // получение ид и пробега авто
        public async Task<Result> GetVehicleAndRepairTypesAsync(int Vehicle_Id, CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                var vehicle = await _dbContex.Vehicles
                    .Include(c => c.RepairTypes)
                    .Select(c => new Vehicle {Id = c.Id, Mileage = c.Mileage,RepairTypes = c.RepairTypes})
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
        }  // получение авто вместе с репаир ид
        public async Task<Result> GetVehicleAndRepairTypesForUpdateAsync(int Vehicle_Id, CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                var vehicle = await _dbContex.Vehicles
                    .Include(c => c.RepairTypes)
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
        } // получение авто для обновления данных
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
        } // удалить авто
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
        } // обновить авто
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
        } // обновление пробега

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
        } // саисок ремонта для машины
        public async Task<Result> GetRepairAsync(int repairId, CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                var repairDb = await _dbContex.Repairs
                    .Include(c => c.RepairType)
                    .FirstOrDefaultAsync(c => c.Id == repairId);

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
        }  // получить список ремонта
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
        }  // создание ремонта
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
        }  // удалить ремонт
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
        }  // обновить ремонта


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
        } // полуичть тпы ремонта список
        public async Task<Result> UpdateRepairTypeAsync(RepairType repaitType, CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                var repairDb = await _dbContex.RepairTypes.FindAsync(repaitType.Id,token);
                if (repairDb != null)
                {
                    repairDb.IntervalMonth = repaitType.IntervalMonth;
                    repairDb.IntervalMileage = repaitType.IntervalMileage;

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
        } // обновить тип ремонта

        //VehicleNotes

        public async Task<Result> ListVehicleNotesAsync(int id,CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                var notes = await _dbContex.VehicleNotes.Where(c => c.VehicleId == id).OrderBy(c => c.DateCreated).ToListAsync(token);
                if (notes != null) return Result<List<VehicleNotes>>.SuccessCreate(notes);
                else return Result.ErrorCreate("Список заметок пуст");
            }
            catch (OperationCanceledException ex)
            {
                return Result.ErrorCreate("Операция была отменена");
            }
            catch (Exception ex)
            {
                return Result.ErrorCreate($"Произошла ошибка при получении списка заметок: {ex.Message}");
            }
        } // получить список заметок

        public async Task<Result> CreateVehicleNotesAsync(VehicleNotes note, CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await _dbContex.VehicleNotes.AddAsync(note, token);
                await _dbContex.SaveChangesAsync(token);
                return Result.SuccessCreate();
            }
            catch (OperationCanceledException ex)
            {
                return Result.ErrorCreate("Операция была отменена");
            }
            catch (Exception ex)
            {
                return Result.ErrorCreate($"Произошла ошибка при сохранении заметки: {ex.Message}");
            }
        }

        public async Task<Result> DeleteVehicleNotesAsync(int id, CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                var respon = await _dbContex.VehicleNotes.FindAsync(id);
                if (respon != null)
                {
                    _dbContex.VehicleNotes.Remove(respon);
                    await _dbContex.SaveChangesAsync();
                    return Result.SuccessCreate();
                }
                else return Result.ErrorCreate("заметка не найдена");

            }
            catch (OperationCanceledException ex)
            {
                return Result.ErrorCreate("Операция была отменена");
            }
            catch (Exception ex)
            {
                return Result.ErrorCreate($"Произошла ошибка при удалении заметки: {ex.Message}");
            }
        }

        public async Task<Result> UpdateVehileNoteAsync(VehicleNotes note, CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                var noteDb = await _dbContex.VehicleNotes.FindAsync(note.Id, token);
                if (noteDb != null)
                {
                    noteDb.Title = note.Title;
                    noteDb.Content = note.Content;
                    noteDb.DateUpdated = note.DateUpdated;

                    await _dbContex.SaveChangesAsync();
                    return Result.SuccessCreate();
                }
                else return Result.ErrorCreate("Заметка не найдена");
            }
            catch (OperationCanceledException ex)
            {
                return Result.ErrorCreate("Операция была отменена");
            }
            catch (Exception ex)
            {
                return Result.ErrorCreate($"Произошла ошибка при обновлении заметки: {ex.Message}");
            }
        }


        public void InitializeDatabase()
        {
            _dbContex.Database.Migrate();
        } // инициализация БД при запуске Приложения
    }
}
