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
        public int IntervalMileage { get; set; } = -1;
        public int IntervalMonth { get; set; } = 0;

        //Последний пробег ремонта авто
        [Required]
        public int LastServiceMileage { get; set; } = 0;

        public string TransmissionType {  get; set; } = string.Empty;

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

        public RepairType(string titleRepair,string category, int intervalMileage, int intervalMonth, int lastServiceMileage,string transmissionType)
        {
            TitleRepair = titleRepair;
            Category = category;
            IntervalMileage = intervalMileage;
            IntervalMonth = intervalMonth;
            LastServiceMileage = lastServiceMileage;
            TransmissionType = transmissionType;
        }
     
        public RepairType(string titleRepair, string category, int intervalMileage)
           : this(titleRepair, category, intervalMileage, default, default, String.Empty)
        {

        }

        public RepairType(string titleRepair, string category, int intervalMileage, int intervalMonth,string transmissionType)
          : this(titleRepair, category, intervalMileage, intervalMonth, default, transmissionType)
        {

        }

        public RepairType(string titleRepair, string category)
          : this(titleRepair, category, default, default,default, String.Empty)
        {

        }


        public RepairType(string titleRepair, string category, int intervalMileage, int intervalMonth)
         : this(titleRepair, category, intervalMileage, intervalMonth , default, String.Empty)
        {

        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
