using AutoCareDiray.Shared.Models.Notes;
using AutoCareDiray.Shared.Models.RepairModel;
using AutoCareDiray.Shared.Models.VehicleModel;
using AutoCareDiray.Shared.Service.ResultService;
using Microsoft.EntityFrameworkCore;

namespace AutoCareDiray.Shared.Interface
{
    public interface IDataService
    {

        //Vehicle
        Task<Result> CreateVehicleAsync(Vehicle vehicle,CancellationToken token);
        Task<Result> ListVehicleAsync(CancellationToken token);
        Task<Result> ListVehicleForListRepairAsync(CancellationToken token);
        Task<Result> GetVehicleAsync(int Vehicle_Id, CancellationToken token);
        Task<Result> GetVehicleMileageAsync(int Vehicle_Id, CancellationToken token);
        Task<Result> GetVehicleAndRepairTypesAsync(int Vehicle_Id, CancellationToken token);
        Task<Result> GetVehicleAndRepairTypesForUpdateAsync(int Vehicle_Id, CancellationToken token);
        Task<Result> DeleteVehicleAsync(Vehicle vehicle, CancellationToken token);
        Task<Result> UpdateVehicleAsync(Vehicle vehicle, CancellationToken token);
        Task<Result> UpdateVehicleMileageAsync(int vehicleId,int Mileage, CancellationToken token);

        //Repair

        Task<Result> ListRepairForVehicleAsync(int VehicleId, CancellationToken token);
        Task<Result> GetRepairAsync(int repairId, CancellationToken token);
        Task<Result> CreateRepairAsync(Repair repair, CancellationToken token);
        Task<Result> DeleteRepairAsync(Repair repair, CancellationToken token);
        Task<Result> UpdateRepairAsync(Repair repair, CancellationToken token);



        //RepairType
        Task<Result> GetListRepairTypeAsync(int vehicleId,CancellationToken token);
        Task<Result> UpdateRepairTypeAsync(RepairType repaitType, CancellationToken token);


        //VehicleNote
        Task<Result> ListVehicleNotesAsync(int Vehicleid, CancellationToken token);
        Task<Result> CreateVehicleNotesAsync(VehicleNotes note, CancellationToken token);
        Task<Result> DeleteVehicleNotesAsync(int id, CancellationToken token);
        Task<Result> UpdateVehileNoteAsync(VehicleNotes note, CancellationToken token);
        void InitializeDatabase();
    }
}
