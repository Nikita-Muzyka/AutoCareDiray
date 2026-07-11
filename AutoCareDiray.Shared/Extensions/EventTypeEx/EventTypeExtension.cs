using AutoCareDiray.Shared.Models.Journal;
using AutoCareDiray.Shared.Models.RepairModel;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCareDiray.Shared.Extensions.EventTypeEx
{
    public static class EventTypeExtension
    {
        public static string GetTypeName(this EventType eventType)
        {
            return eventType switch
            {
                EventType.Repair => "Ремонт",
                EventType.Refill => "Заправка",
                EventType.AllEvent => "Все события",
                _ => "Все категории",
            };
        }
    }
}
