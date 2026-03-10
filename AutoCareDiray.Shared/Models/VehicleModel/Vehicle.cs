using AutoCareDiray.Shared.Models.RepairModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace AutoCareDiray.Shared.Models.VehicleModel
{
    public class Vehicle
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string NameVehicle { get; set; } = String.Empty;
        public DateTime YearCreate { get; set; }
        public DateTime YearPurchase { get; set; }

        [Required]
        public string VehicleType { get; set; } = String.Empty;

        [Required]
        public int Mileage { get; set; }

        public List<Repair> Repairs { get; set; } = new();
        public List<RepairType> ReepairTypes { get; set; } = new();
    }
}
