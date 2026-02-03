using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCareDiray.Service
{
    public interface IDialogService
    {
        Task<bool> ShowConfirmationMessage(string message);
        Task ShowMessage(string message);
        Task ShowInfo(string message);
    }
}
