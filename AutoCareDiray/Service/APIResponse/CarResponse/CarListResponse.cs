using AutoCareDiray.Models;
using AutoCareDiray.Service;
namespace AutoCareDiray.Services
{
    public class CarListResponse : ApiResponse
    {
        public IEnumerable<Car> cars {  get; set; }
        public CarListResponse(string message,bool success,IEnumerable<Car> cars) : base(message, success)
        {
            this.cars = cars;
        }
    }
}
