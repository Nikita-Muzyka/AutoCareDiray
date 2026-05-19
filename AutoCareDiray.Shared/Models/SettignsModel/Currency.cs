using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCareDiray.Shared.Models.SettignsModel
{
    public class Currency
    {

        public Currency(string code,string title,string sign)
        {
            Code = code;
            Title = title;
            Sign = sign;
        }
        public string Code { get; set; }
        public string Title { get; set; }
        public string Sign { get; set; }

    }
}
