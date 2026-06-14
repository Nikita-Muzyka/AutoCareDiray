using AutoCareDiray.Shared.Models.RepairModel;
using AutoCareDiray.Shared.Models.Notes;
using AutoCareDiray.Shared.Models.RefillModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using AutoCareDiray.Shared.Models.SettignsModel;



namespace AutoCareDiray.Shared.Models.VehicleModel
{
    public class Vehicle : INotifyPropertyChanged
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string? NameVehicle { get; set; }
        public string? VinCode { get; set; } //вин код
        public string? StateNumber { get; set; } //нормер гос
        public string? TransmissionType { get; set; } //тип трансмиссии
        public string? WarningRepair { get; set; }    //Кол-во предупреждений по машине
        [Required]
        public string UnitDistance { get; set; } //какая величина растояния у авто
        [Required]
        public string UnitVolume { get; set; } //какая величина авто
        [Required]
        public string? VehicleType { get; set; }

        public DateTime YearPurchase { get; set; } = new DateTime(1970, 1, 1);


        [Required]
        public double FuelTank { get; set; }    //бак авто
        public double VolumeLiters { get; set; } // кол-во литров в баке
        [Required]
        public double Mileage { get; set; } = 0;

        public List<Repair> Repairs { get; set; } = new();
        public List<VehicleNotes> Notes { get; set; } = new();
        public List<Refill> Refills { get; set; } = new();
        public List<RepairType> RepairTypes { get; set; } = new();


        private string photoVehicle;
        public string? PhotoVehicle
        {
            get => photoVehicle;
            set
            {
                if (photoVehicle != value)
                {
                    photoVehicle = value;
                    OnPropertyChanged();
                }
            }
        } //Путь к фото авто

        private string? _pdfFile;
        public string? PdfFile 
        {
            get => _pdfFile;
            set
            {
                if (_pdfFile != value)
                {
                    _pdfFile = value;
                    OnPropertyChanged();
                }
            }
        } //Pdf file путь}

        private bool _needsService;
        public bool NeedsService
        {
            get => _needsService;
            set
            {
                if (_needsService != value)
                {
                    _needsService = value;
                    OnPropertyChanged();
                }
            }
        } //по нему выбирается цвет индикатора авто в lIst


        [NotMapped]
        public string DisplayMileage => $"{Mileage:N0} {UnitDistance}";


        public Vehicle() { }

        public Vehicle(string name,
            DateTime yearPurchase,
            string vinCode,
            string stateNumber,
            string transmissionType, 
            string vehicleType,
            double mileage,double fuelTank,
            string uDist,string uVolume) 
        {
            NameVehicle = name;
            YearPurchase = yearPurchase;
            VinCode = vinCode;
            StateNumber = stateNumber;
            TransmissionType = transmissionType;
            VehicleType = vehicleType;
            Mileage = mileage;
            FuelTank = fuelTank;
            UnitDistance = uDist;
            UnitVolume = uVolume;
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    }
}
