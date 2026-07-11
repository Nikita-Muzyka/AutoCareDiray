using AutoCareDiray.Shared.Models.RepairModel;
using AutoCareDiray.Shared.Models.SettignsModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCareDiray.Shared.Models.Journal
{
    public class TimelineEvent
    {
        public int RecordId { get; set; } // Хранит Id оригинального ремонта или заправки
        public EventType Type { get; set; } // типы евентов
        public RepairCategory? RepairCategory { get; set; }

        public Currency Currency { get; set; }

        // Визуальные данные (то, что видит пользователь)
        public DateTime Date { get; set; }
        public string Title { get; set; }      // название типа
        public string Subtitle { get; set; }   // "Лукойл • 40 л."

        public string Mileage { get; set; }   
        public decimal Cost { get; set; }      // Чтобы вывести сумму в карточке
        public string IconSource { get; set; }

        // Для красивого вывода суммы в XAML можно сделать готовое свойство:
        public string FormattedCost => $"- {Cost:N0} ";
    }
}
