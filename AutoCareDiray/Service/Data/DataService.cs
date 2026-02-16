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

        public async Task CreateVehicleAsync(Vehicle vehicle)
        {
            try
            {
                await _dbContex.Vehicles.AddAsync(vehicle);
                await _dbContex.SaveChangesAsync();
            }
            catch (Exception ex)
            {

            }

        }
        public async Task<IEnumerable<Vehicle>> ListVehicleAsync()
        {
            try
            {
                var vehicles = await _dbContex.Vehicles.ToListAsync();
                return vehicles;
            }
            catch (Exception ex)
            {
                return Enumerable.Empty<Vehicle>();
            }
        }

        public void InitializeDatabase()
        {
            _dbContex.Database.Migrate();
        }
    }
}
