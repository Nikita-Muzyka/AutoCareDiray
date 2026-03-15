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
        public string VinCode { get; set; } = String.Empty;
        public string StateNumber { get; set; } = String.Empty;
        public string TransmissionType { get; set; } = String.Empty;

        [Required]
        public string VehicleType { get; set; } = String.Empty;

        [Required]
        public int Mileage { get; set; }

        public List<Repair> Repairs { get; set; } = new();
        public List<RepairType> RepairTypes { get; set; } = new();

        public Vehicle() { }

        public Vehicle(string name,
            DateTime yearCreate, 
            DateTime yearPurchase,
            string vinCode,
            string stateNumber,
            string transmissionType, 
            string vehicleType,
            int mileage,
            List<RepairType> repairTypes) 
        {
            NameVehicle = name;
            YearCreate = yearCreate;
            YearPurchase = yearPurchase;
            VinCode = vinCode;
            StateNumber = stateNumber;
            TransmissionType = transmissionType;
            VehicleType = vehicleType;
            Mileage = mileage;
            RepairTypes = repairTypes;
        }
    }
}
