using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCareDiray.Shared.Interface
{
    public interface IPhotoPicker
    {
        Task<string> PickPhotoAsync();
    }
}
