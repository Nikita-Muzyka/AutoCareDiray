using AutoCareDiray.Shared.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCareDiray.Service.Navigation
{
    public class NavigationService : INavigationService
    {
        public async Task GoNavigation(string roud)
        {
            var shell = Shell.Current;
            if (shell is not null)
            {
                await Shell.Current.GoToAsync(roud);
            }
        }

        public async Task GoNavigation(string roud, IDictionary<string, object> paramentr)
        {
            var shell = Shell.Current;
            if (shell is not null)
            {
                await Shell.Current.GoToAsync(roud, paramentr);
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

