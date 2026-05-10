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
        public async Task<Result> GetListVehicleAsync(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                var vehicles = await _dbContex.Vehicles
                    .AsNoTracking()
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
        public async Task<Result> GetListVehicleForListRepairAsync(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                var vehicles = await _dbContex.Vehicles
                    .AsNoTracking()
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
                    .AsNoTracking()
                    .Include(c=> c.RepairTypes)
                    .Include(c => c.Repairs)
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
                    .AsNoTracking()
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
                    .AsNoTracking()
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
                    .AsNoTracking()
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
                    _dbContex.Entry(vehicleDb).CurrentValues.SetValues(vehicle);
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
                int countUpdate = await _dbContex.Vehicles
                    .Where(c => c.Id == vehicleId)
                    .ExecuteUpdateAsync(s => s.SetProperty(r => r.Mileage, mileage), token);

                if (countUpdate > 0) return Result.SuccessCreate();
                else return Result.ErrorCreate("Ошибка обновления пробега авто");
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
        public async Task<Result> UpdateVehiclePdfAsync(int vehicleId, string pdfFile, CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                int countUpdate = await _dbContex.Vehicles
                    .Where(c => c.Id == vehicleId)
                    .ExecuteUpdateAsync(s => s.SetProperty(r => r.PdfFile, pdfFile),token);

                if (countUpdate > 0)
                {
                    return Result.SuccessCreate();
                }
                else return Result.ErrorCreate("Ошибка при обновлении");
            }
            catch (OperationCanceledException)
            {
                return Result.ErrorCreate("Операция была отменена");
            }
            catch (Exception ex)
            {
                return Result.ErrorCreate($"Произошла ошибка при обновлении PDF: {ex.Message}");
            }
        } // обновление PDF

        //Repair

        public async Task<Result> GetListRepairForVehicleAsync(int VehicleId, CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                var repairs = await _dbContex.Repairs
                    .AsNoTracking()
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
                    .AsNoTracking()
                    .Include(c => c.RepairType)
                    .Include(c => c.Vehicle)
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
        }  // получить ремонт со списком типов ремонтов
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
                    _dbContex.Entry(repairDb).CurrentValues.SetValues(repair);

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
                var repairTypes = await _dbContex.RepairTypes
                    .AsNoTracking()
                    .Where(c => c.VehicleId == vehicleId)
                    .AsNoTracking()
                    .ToListAsync();
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
                    _dbContex.Entry(repairDb).CurrentValues.SetValues(repaitType);

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

        public async Task<Result> GetListVehicleNotesAsync(int id,CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                var notes = await _dbContex.VehicleNotes
                    .AsNoTracking()
                    .Where(c => c.VehicleId == id)
                    .OrderBy(c => c.DateCreated)
                    .ToListAsync(token);
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
        } // Создать заметку
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
        } // удалить заметку
        public async Task<Result> UpdateVehileNoteAsync(VehicleNotes note, CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                var noteDb = await _dbContex.VehicleNotes.FindAsync(note.Id, token);
                if (noteDb != null)
                {
                    _dbContex.Entry(noteDb).CurrentValues.SetValues(note);

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
        } // обновить заметку


        public void InitializeDatabase()
        {
            _dbContex.Database.Migrate();
        } // инициализация БД при запуске Приложения
    }
}
