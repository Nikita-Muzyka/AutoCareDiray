using AutoCareDiray.Shared.Models.VehicleModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCareDiray.Shared.Models.RefillModel
{
    public class Refill
    {
        // Обязательные ключи
        public int Id { get; set; }
       
        // Основные данные заправки
        public DateTime DateRefill { get; set; }
        public int Mileage { get; set; } // Пробег на момент заправки
        public double VolumeLiters { get; set; } // Количество залитых литров
        public decimal Cost { get; set; } // Общая стоимость (лучше decimal для денег)

        // Дополнительные свойства
        public string FuelType { get; set; } // Например: "АИ-95", "ДТ"
        public bool IsFullTank { get; set; } // Заправлен ли до полного (для графиков)
        public string? GasStationName { get; set; } // Бренд заправки
        public string? Description { get; set; } // Личные заметки

        public string? PhotoPaths { get; set; }


        [Required]
        public int VehicleId { get; set; }
        public Vehicle? Vehicle { get; set; }

    }
}
