using AutoCareDiray.Shared.Models.Notes;
using AutoCareDiray.Shared.Models.RefillModel;
using AutoCareDiray.Shared.Models.RepairModel;
using AutoCareDiray.Shared.Models.VehicleModel;
using AutoCareDiray.Shared.Service.ResultService;
using Microsoft.EntityFrameworkCore;

namespace AutoCareDiray.Shared.Interface
{
    public interface IDataService
    {

        //Vehicle 
        Task<Result> GetFullVehicleAsync(int vehicleId, CancellationToken token); // получение авто со всеми связными таблицами без заметок и типов ремонта
        Task<Result> CreateVehicleAsync(Vehicle vehicle,CancellationToken token); // Создание авто
        Task<Result> GetListVehicleAsync(CancellationToken token); // список авто вместе с типоми ремонта
        Task<Result> GetListVehicleNameAsync(CancellationToken token); // список авто только название и ид
        Task<Result> GetVehicleAsync(int Vehicle_Id, CancellationToken token); // получение авто
        Task<Result> GetVehicleMileageAsync(int Vehicle_Id, CancellationToken token); // получение ид и пробега авто
        Task<Result> GetVehicleAndRepairTypesAsync(int Vehicle_Id, CancellationToken token); // получение авто вместе с репаир ид
        Task<Result> GetVehicleAndRepairTypesForUpdateAsync(int Vehicle_Id, CancellationToken token); // получение авто для обновления данных
        Task<Result> DeleteVehicleAsync(Vehicle vehicle, CancellationToken token); // удалить авто
        Task<Result> UpdateVehicleAsync(Vehicle vehicle, CancellationToken token); // обновить авто
        Task<Result> UpdateVehicleMileageAsync(int vehicleId,int Mileage, CancellationToken token); // обновление пробега
        Task<Result> UpdateVehiclePdfAsync(int vehicleId, string pdfFile, CancellationToken token); // обновление PDF


        //Repair

        Task<Result> GetListRepairForVehicleAsync(int VehicleId, CancellationToken token); // саисок ремонта для машины
        Task<Result> GetRepairAsync(int repairId, CancellationToken token); // получить список ремонта
        Task<Result> CreateRepairAsync(Repair repair, CancellationToken token); // создание ремонта
        Task<Result> DeleteRepairAsync(int repairId, CancellationToken token); // удалить ремонт
        Task<Result> UpdateRepairAsync(Repair repair, CancellationToken token);  // обновить ремонта



        //RepairType

        Task<Result> GetRepairTypeAsync(RepairType repaitType, CancellationToken token); // получить тип ремонта
        Task<Result> GetListRepairTypeAsync(int vehicleId,CancellationToken token); // полуичть тпы ремонта список
        Task<Result> CreateRepairTypeAsync(RepairType repairType, CancellationToken token); // добавление RepairType
        Task<Result> UpdateRepairTypeAsync(RepairType repaitType, CancellationToken token); // обновить тип ремонта

        //VehicleNote
        Task<Result> GetListVehicleNotesAsync(int Vehicleid, CancellationToken token); // получить список заметок
        Task<Result> CreateVehicleNotesAsync(VehicleNotes note, CancellationToken token); // Создать заметку
        Task<Result> DeleteVehicleNotesAsync(int id, CancellationToken token); // удалить заметку
        Task<Result> UpdateVehileNoteAsync(VehicleNotes note, CancellationToken token); // обновить заметку

        //Refill
        Task<Result> GetRefillAsync(int refillId, CancellationToken token); // получить данные о заправке
        Task<Result> UpdateRefillAsync(Refill refill, CancellationToken token); // обновление данные о заправке
        Task<Result> CreateRefillAsync(Refill refill, CancellationToken token); // Создание данные о заправке
        Task<Result> DeleteRefillAsync(int refillId, CancellationToken token); // Создание данные о заправке


        void InitializeDatabase(); // инициализация БД при запуске Приложения
    }
}
