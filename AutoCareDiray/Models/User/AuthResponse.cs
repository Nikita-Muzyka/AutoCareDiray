using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AutoCareDiray.Models
{
    public class AuthResponse
    {
        public bool? Success { get; set; }
        public string? Message { get; set; }
        public string? NickName { get; set; }
        public string? Email { get; set; }

        public AuthResponse() { }

        public AuthResponse(bool success, string message)
        {
            Success = success;
            Message = message;
        }
        public AuthResponse(bool success, string message, string nickName, string email) : this(success, message)
        {
            NickName = nickName;
            Email = email;
        }

    }
}
