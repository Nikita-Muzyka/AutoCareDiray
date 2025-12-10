using AutoCareDiray.Service;

namespace AutoCareDiray.Services
{
    public class GetCarResponse : ApiResponse
    {
        public int Car_Id { get; set; }
        public string Brand {  get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string? Vin { get; set; }
        public int Mileage { get; set; }
        public int YearPurchase { get; set; }
        public string? TransmissionBox { get; set; }
        public string? EngineType { get; set; }

        public GetCarResponse(
            string message,
            int car_id,
            string brand,
            string model,
            int year,
            string? vin,
            int mileage,
            int yearPurchase,
            string? transmissionBox,
            string? engineType) : base(message, true) 
        {
            Car_Id = car_id;
            Brand = brand;
            Model = model;
            Year = year;
            Vin = vin;
            Mileage = mileage;
            YearPurchase = yearPurchase;
            TransmissionBox = transmissionBox;
            EngineType = engineType;
        }
    }
}
