using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCareDiray.Shared.Data
{
    public class AppDBContexFactory : IDesignTimeDbContextFactory<AppDBContex>
    {
        public AppDBContex CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDBContex>();

            // ✅ Путь к БД: используем тот же, что и в приложении
            var dbPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "vehiclesApp.db");

            optionsBuilder.UseSqlite($"Data Source={dbPath}");

            return new AppDBContex(optionsBuilder.Options);
        }
    }
}
