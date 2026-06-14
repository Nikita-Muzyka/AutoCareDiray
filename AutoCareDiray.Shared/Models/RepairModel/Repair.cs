using AutoCareDiray.Shared.Models.VehicleModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCareDiray.Shared.Models.RepairModel
{
    public class Repair
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public double CurrentMileage { get; set; }
        public DateTime DateRepair { get; set; }
        public List<SparePart>? SpareParts { get; set; }
        public decimal Cost { get; set; } = default;
        public string? Description { get; set; }
        public string? ServiceName { get; set; }
        public string? CommentMechanic { get; set; }
        public string? Job { get; set; }
        public List<string>? Photos { get; set; }

        public string? ProgressPercentMileage
        {
            get
            {
                return $"Износ по пробегу {Math.Round(ProgressMileage,1) * 100} %";
            }
        }
        public string? ProgressPercentMonth
        {
            get
            {
                return $"Износ по времени {Math.Round(ProgressMonth, 1) * 100} %";
            }
        }
        public double ProgressMileage
        {
            get
            {
                if (Vehicle.Mileage > 0)
                {
                    var newMileage = Vehicle.Mileage - CurrentMileage;
                    var progress = (double)newMileage / (double)RepairType.IntervalMileage;
                    var result = Math.Min(progress, 1);
                    return result;
                }
                else return 0;
            }
        }
        public double ProgressMonth
        {
            get
            {
                if (RepairType.IntervalMonth > 0)
                {
                    var day = DateTime.UtcNow - DateRepair;
                    var progress = ((double)day.Days / 30) / (double)RepairType.IntervalMonth;
                    var result = Math.Min(progress, 1);
                    return result;
                }
                else return 0;
            }
        }


        [Required]
        public int VehicleId { get; set; }
        public Vehicle? Vehicle { get; set; }

        [Required]
        public int RepairTypeId { get; set; }
        public RepairType? RepairType { get; set; }
    }
}
