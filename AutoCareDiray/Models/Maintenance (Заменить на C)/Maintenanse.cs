using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCareDiray.Models
{
    public class MaintenanseModel
    {
        public int Maintenanse_Id { get; set; }
        public int Car_Id { get; set; }
        public DateTime ServiceDate { get; set; }
        public int Mileage { get; set; }
        public string ServiceType { get; set; }
        public string Description { get; set; }
        public int? Cost { get; set;}
        public string? ServiceCentre { get; set; }

        public MaintenanseModel(int car_Id, DateTime serviceDate, int mileage, string serviceType, string description, int cost, string serviceCentre)
        {
            Car_Id = car_Id;
            ServiceDate = serviceDate;
            Mileage = mileage;
            ServiceType = serviceType;
            Description = description;
            Cost = cost;
            ServiceCentre = serviceCentre;
        }
    }
}
