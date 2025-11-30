using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCareDiray.Models
{
    public class CarResponse
    {
        public string Message { get; set; }
        public bool Success { get; set; }

        public int Car_id { get; set; }
        public int User_id { get; set; }

        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string? Vin { get; set; }

        public int? Current_mileage { get; set; }
        public int? Year_purchase { get; set; }

        public string? Transmission_box { get; set; }
        public string? Engine_type { get; set; }

    }
}
