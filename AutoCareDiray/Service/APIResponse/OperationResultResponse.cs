using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCareDiray.Service
{
    public class OperationResultResponse : ApiResponse
    {
        public OperationResultResponse(string message, bool succees) : base(message, succees) { }
    }
}
