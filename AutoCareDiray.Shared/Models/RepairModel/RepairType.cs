using AutoCareDiray.Shared.Extensions.RepairEx;
using AutoCareDiray.Shared.Interface;
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
        public string? TitleRepair { get; set; }
        public string TransmissionType { get; set; } = string.Empty;

        [Required]
        public int IntervalMileage { get; set; } = 0;
        public int IntervalMonth { get; set; } = 0;

        [Required]
        public double LastServiceMileage { get; set; } = 0;  //Последний пробег ремонта авто

        public DateTime LastServiceDate { get; set; } = default; //Последняя дата ремонта авто

        public RepairCategory Category { get; set; }
        public string CategoryText
        {
            get { return Category.GetDisplay();  }
        }

        //Выбор обслужена категория или нет
        private bool _isServiced = false;
        public bool IsServiced
        {
            get => _isServiced;
            set
            {
                if(Vehicle != null)
                {
                    if (value == true)
                    {
                        if (_isServiced == true) { }
                        else
                        {
                            LastServiceMileage = Vehicle.Mileage;
                            LastServiceDate = DateTime.UtcNow;
                        }
                    }
                    else
                    {
                        LastServiceMileage = default;
                        LastServiceDate = default;
                    }
                }
                else
                {
                    if (value == true)
                    {
                        if (_isServiced == true) { }
                        else
                        {
                            LastServiceDate = DateTime.UtcNow;
                        }
                    }
                    else
                    {
                        LastServiceMileage = default;
                        LastServiceDate = default;
                    }
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
        public Vehicle? Vehicle { get; set; }

        public List<Repair>? Repairs { get; set; } = new();

        public RepairType() { }

        public RepairType(string titleRepair,RepairCategory category, int intervalMileage, int intervalMonth, int lastServiceMileage,string transmissionType)
        {
            TitleRepair = titleRepair;
            Category = category;
            IntervalMileage = intervalMileage;
            IntervalMonth = intervalMonth;
            LastServiceMileage = lastServiceMileage;
            TransmissionType = transmissionType;
        }
     
        public RepairType(string titleRepair, RepairCategory category, int intervalMileage)
           : this(titleRepair, category, intervalMileage, default, default, String.Empty)
        {

        }

        public RepairType(string titleRepair, RepairCategory category, int intervalMileage, int intervalMonth,string transmissionType)
          : this(titleRepair, category, intervalMileage, intervalMonth, default, transmissionType)
        {

        }

        public RepairType(string titleRepair, RepairCategory category)
          : this(titleRepair, category, default, default,default, String.Empty)
        {

        }


        public RepairType(string titleRepair, RepairCategory category, int intervalMileage, int intervalMonth)
         : this(titleRepair, category, intervalMileage, intervalMonth , default, String.Empty)
        {

        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
