using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCareDiray.Service
{
    public class ErrorsResponse : ApiResponse
    {
        public string ExceptionMessage { get; set; }
        public ErrorsResponse(string message, string exceptionMessage) : base(message, false)
        {
            ExceptionMessage = exceptionMessage;
        }
    }
}
