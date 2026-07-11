using AutoCareDiray.Shared.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCareDiray.Service.MainThreadService
{
    public class MainThreadService : IMainThreadService
    {
        public void RunUIThread(Action action)
        {
            MainThread.BeginInvokeOnMainThread(action);
        }
    }
}
