using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCareDiray.Models.VehicleModel
{
    public class Vehicle
    {
        public int? User_Id { get; set; }
        public string? NameVehicle { get; set; }
        public string? YearCreate { get; set; }
        public int? Mileage { get; set; }
        public string? YearPuchase { get; set; }
    }
}
