using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCareDiray.Shared.Models.RepairModel
{
    public class SparePart
    {
        public SparePart() { }

        public int id { get; set; }
        public string NamePart {  get; set; }
        public string ArticleNumberPart { get; set; }
        public decimal CostPart { get; set; }
        public string CurrentCurrencySing { get; set; }
        public string DisplayCost 
        {
            get => CostPart.ToString() + " " + CurrentCurrencySing;
            set { }
        }
    }
}
