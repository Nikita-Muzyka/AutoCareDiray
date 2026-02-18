using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCareDiray.Service.Navigation
{
    public class NavigationService : INavigationService
    {
        public async Task GoNavigation(ShellNavigationState ShellState)
        {
            var shell = Shell.Current;
            if (shell is not null)
            {
                await Shell.Current.GoToAsync(ShellState);
            }
        }

        public async Task GoNavigation(ShellNavigationState ShellState, IDictionary<string, object> paramentr)
        {
            var shell = Shell.Current;
            if (shell is not null)
            {
                await Shell.Current.GoToAsync(ShellState, paramentr);
            }
        }

        public async Task GoToBack()
        {
            var shell = Shell.Current;
            if(shell is not null)
            {
                await Shell.Current.GoToAsync("..");
            }
        }
    }
}

