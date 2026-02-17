using AutoCareDiray.Shared.Data;
using AutoCareDiray.Shared.Models.VehicleModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCareDiray.Service.Data
{
    public class DataService : IDataService
    {
        private readonly AppDBContex _dbContex;
        public DataService(AppDBContex db)
        {
            _dbContex = db;
        }

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
        public async Task<IEnumerable<Vehicle>> ListVehicleAsync(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                var vehicles = await _dbContex.Vehicles.ToListAsync(token);
                return vehicles;
            }
            catch (OperationCanceledException)
            {
                return Enumerable.Empty<Vehicle>();
            }
            catch (Exception ex)
            {
                return Enumerable.Empty<Vehicle>();
            }
        }

        public async Task<Vehicle> GetVehicleAsync(int Vehicle_Id, CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                var vehicle = await _dbContex.Vehicles.FirstOrDefaultAsync(v => v.Vehicle_Id == Vehicle_Id, token);
                return vehicle;
            }
            catch (OperationCanceledException)
            {
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public void InitializeDatabase()
        {
            _dbContex.Database.Migrate();
        }
    }
}
