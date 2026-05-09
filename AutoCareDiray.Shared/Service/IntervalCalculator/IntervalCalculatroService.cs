using AutoCareDiray.Shared.Models.RepairModel;
using AutoCareDiray.Shared.Models.VehicleModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AutoCareDiray.Shared.Service.IntervalCalculator
{
    public static class IntervalCalculatroService
    {
        public static IEnumerable<RepairType> CalculatingWarningList(Vehicle vehicle)
        {
            var sortRepairType = vehicle.RepairTypes.Where(c => c.IntervalMileage > 0 && c.IntervalMonth > 0).ToList();
            var Sort = sortRepairType.Where(c => vehicle.Mileage - c.LastServiceMileage > c.IntervalMileage).ToList();

            foreach (var pair in sortRepairType)
            {
                if (pair.LastServiceDate < new DateTime(1970)) continue;

                var day = DateTime.UtcNow - pair.LastServiceDate;
                if (day.Days / 30 > pair.IntervalMonth)
                {
                    if (Sort.Any(c => c.Id == pair.Id)) continue;
                    Sort.Add(pair);
                }
            }


            return Sort;
        } // подсчет предупреждения узлов требует осмотра созвращает список классов

        public static int CalculatingWarning(Vehicle vehicle)
        {
            var sortRepairType = vehicle.RepairTypes.Where(c => c.IntervalMileage > 0 && c.IntervalMonth > 0).ToList();
            var Sort = sortRepairType.Where(c => vehicle.Mileage - c.LastServiceMileage > c.IntervalMileage).ToList();

            foreach (var pair in sortRepairType)
            {
                if (pair.LastServiceDate < new DateTime(1970)) continue;

                var day = DateTime.UtcNow - pair.LastServiceDate;
                if (day.Days / 30 > pair.IntervalMonth)
                {
                    if (Sort.Any(c => c.Id == pair.Id)) continue;
                    Sort.Add(pair);
                }
            }


            return Sort.Count;
        } // подсчет предупреждения узлов требует осмотра возвращает int
    }
}
