using AutoCareDiray.Shared.Models.VehicleModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        public int CurrentMileage { get; set; }
        public DateTime DateRepair { get; set; }
        public string? SpareParts { get; set; } = String.Empty;
        public int Cost { get; set; } = default;
        public string? Description { get; set; } = String.Empty;
        public string? ServiceName { get; set; } = String.Empty;
        public string? CommentMechanic { get; set; } = String.Empty;
        public string? Job { get; set; } = String.Empty;

        public string? ProgressPercent
        {
            get
            {
                return $"До след ремонта {Math.Round(ProgressMileage,1) * 100} %";
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


        [Required]
        public int VehicleId { get; set; }
        public Vehicle? Vehicle { get; set; }

        [Required]
        public int RepairTypeId { get; set; }
        public RepairType? RepairType { get; set; }
    }
}
