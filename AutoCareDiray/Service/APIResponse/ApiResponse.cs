using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCareDiray.Service
{
    public abstract class ApiResponse
    {
        public string Message { get; set; }
        public bool Success { get; set; }

        public ApiResponse(string message, bool success)
        {
            Message = message;
            Success = success;
        }
    }
}
