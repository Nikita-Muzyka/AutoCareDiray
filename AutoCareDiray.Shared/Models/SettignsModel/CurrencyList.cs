using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCareDiray.Shared.Models.SettignsModel
{
    public static class CurrencyList
    {
        public static List<Currency> GetCurrency()
        {
            return new List<Currency>()
            {
                new Currency("RUB","Рубли","₽"),
                new Currency("BYR","Белорусский рубль","Br"),
                new Currency("USD","Доллар","$"),
                new Currency("EUR","Евро ","€")
            };
        }

        public static Currency GetCurrencyByCode(string code)
        {
            return GetCurrency().FirstOrDefault(c => c.Code == code) ?? GetCurrency().First();
        }
    }
}
