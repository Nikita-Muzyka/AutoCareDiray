using AutoCareDiray.Shared.Models.RepairModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;



namespace AutoCareDiray.Shared.Models.VehicleModel
{
    public class Vehicle : INotifyPropertyChanged
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string PhotoVehicle { get; set; } = String.Empty;
        public string NameVehicle { get; set; } = String.Empty;
        public DateTime YearCreate { get; set; }
        public DateTime YearPurchase { get; set; }
        public string VinCode { get; set; } = String.Empty; //вин код
        public string StateNumber { get; set; } = String.Empty; //нормер гос
        public string TransmissionType { get; set; } = String.Empty; //тип трансмиссии
        public string? WarningRepair { get; set; } = String.Empty;    //Кол-во предупреждений по машине

        [Required]
        public string VehicleType { get; set; } = String.Empty;
        [Required]
        public int Mileage { get; set; }

        public List<Repair> Repairs { get; set; } = new();
        public List<RepairType> RepairTypes { get; set; } = new();


        private string _pdfFile = String.Empty;
        public string PdfFile 
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


        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    }
}
