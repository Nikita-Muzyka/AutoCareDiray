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
    public class RepairType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string TitleRepair { get; set; }
        [Required]
        public int IntervalMileagee { get; set; } = 0;
        [Required]
        public DateTime IntervalDate { get; set; }

        [Required]
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }

        public List<Repair> Repairs { get; set; } = new();

        public RepairType() { }

        public RepairType(string titleRepair, int intervalMileagee, DateTime intervalDate)
        {
            TitleRepair = titleRepair;
            IntervalMileagee = intervalMileagee;
            IntervalDate = intervalDate;
        }
        public RepairType(string titleRepair, int intervalMileagee) : this(titleRepair, intervalMileagee, new DateTime())
        {

        }
    }
}
