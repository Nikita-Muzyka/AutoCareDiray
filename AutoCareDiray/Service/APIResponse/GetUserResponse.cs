using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCareDiray.Service
{
    public class GetUserResponse : ApiResponse
    {
        public int User_Id { get; set; }
        public string NickName { get; set; }
        public string Email { get; set; }

        public GetUserResponse(int user_Id, string nickName, string email, string message) : base(message, true)
        {
            User_Id = user_Id;
            NickName = nickName;
            Email = email;
        }
    }
}
