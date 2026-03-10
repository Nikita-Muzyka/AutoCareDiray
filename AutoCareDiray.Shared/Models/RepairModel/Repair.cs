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


        [Required]
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }

        [Required]
        public int RepairTypeId { get; set; }
        public RepairType RepairType { get; set; }
    }
}
