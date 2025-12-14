using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoCareDiray.Service;
using AutoCareDiray.View;

namespace AutoCareDiray.ViewModels
{
    public partial class MainPageViewModal : BaseViewModel
    {
        public MainPageViewModal(IApiService apiService,IDialogService dialogService) : base(apiService,dialogService)
        {

        }
    }
}
