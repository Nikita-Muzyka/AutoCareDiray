using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AutoCareDiray.Models
{
    public class UserResponse
    {
        public int? User_id { get; set; }
        public bool? Success { get; set; }
        public string? Message { get; set; }
        public string? NickName { get; set; }
        public string? Email { get; set; }

        public UserResponse() { }
    }
}
