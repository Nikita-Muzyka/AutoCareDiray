using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCareDiray.Service.Navigation
{
    public interface INavigationService
    {
        Task GoNavigation(ShellNavigationState ShellState);

        Task GoNavigation(ShellNavigationState ShellState,IDictionary<string,object> paramentr);

        Task GoToBack();
    }
}
