using AutoCareDiray.Shared.Models.VehicleModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AutoCareDiray.Shared.Models.RepairModel
{
    public class RepairType : INotifyPropertyChanged
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string TitleRepair { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public int IntervalMileage { get; set; } = 0;

        //Последний пробег ремонта авто
        [Required]
        public int LastServiceMileage { get; set; } = 0;

        public string TransmissionType {  get; set; } = string.Empty;
        public DateTime? IntervalDate { get; set; }

        //Выбор обслужена категория или нет
        private bool _isServiced = false;
        public bool IsServiced
        {
            get => _isServiced;
            set
            {
                if(value == true && Vehicle != null)
                {
                    if(_isServiced == true) { }
                    else LastServiceMileage = Vehicle.Mileage;
                }
                else if (value == false && Vehicle != null)
                {
                    LastServiceMileage = default;
                }
                _isServiced = value;
                    OnPropertyChanged();
            }
        }
        //убрать запись из обслуживания или нет
        private bool _isRemoveMaintenance = false;
        public bool IsRemoveMaintenance
        {
            get => _isRemoveMaintenance;
            set
            {
                _isRemoveMaintenance= value; 
                OnPropertyChanged();
            }
        }


        [Required]
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }

        public List<Repair> Repairs { get; set; } = new();

        public RepairType() { }

        public RepairType(string titleRepair,string category, int intervalMileage, int lastServiceMileage,string transmissionType, DateTime intervalDate)
        {
            TitleRepair = titleRepair;
            Category = category;
            IntervalMileage = intervalMileage;
            IntervalDate = intervalDate;
            LastServiceMileage = lastServiceMileage;
            TransmissionType = transmissionType;
        }
        public RepairType(string titleRepair,string category, int intervalMileage, int lastServiceMileage) 
            : this(titleRepair,category, intervalMileage, lastServiceMileage,String.Empty, new DateTime())
        {

        }

        public RepairType(string titleRepair, string category, int intervalMileage)
           : this(titleRepair, category, intervalMileage, default, String.Empty, new DateTime())
        {

        }

        public RepairType(string titleRepair, string category, int intervalMileage,string transmissionType)
          : this(titleRepair, category, intervalMileage, default, transmissionType, new DateTime())
        {

        }

        public RepairType(string titleRepair, string category)
          : this(titleRepair, category, default, default, String.Empty, new DateTime())
        {

        }

        public RepairType(string titleRepair, string category, int intervalMileage, DateTime intervalDate)
         : this(titleRepair, category, intervalMileage, default, String.Empty, intervalDate)
        {

        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
