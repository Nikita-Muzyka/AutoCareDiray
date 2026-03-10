using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCareDiray.Shared.Interface
{
    public interface INavigationService
    {
        Task GoNavigation(string roud);

        Task GoNavigation(string roud, IDictionary<string,object> paramentr);

        Task GoToBack();
    }
}
