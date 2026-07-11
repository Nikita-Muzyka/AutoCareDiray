using AutoCareDiray.Shared.Models.Journal;
using AutoCareDiray.Shared.Models.RepairModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCareDiray.Shared.Extensions.StringEx
{
    public static class StringEventTypeExtension
    {
        public static EventType GetEventType(this string eventType)
        {
            return eventType switch
            {
                "Ремонт" => EventType.Repair,
                "Заправка" => EventType.Refill,
                "Все события" => EventType.AllEvent,
            };
        }
    }
}
