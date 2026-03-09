using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCareDiray.Shared.Interface
{
    public interface IValidatorService
    {
        void ErrorAdd(string propertyName,string value);
        void ErrorRemove(string propertyName);
        void OnErrorsChanges(string propertyName);
    }
}
