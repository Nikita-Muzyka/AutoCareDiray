namespace AutoCareDiray.Shared.DTOs.UserDTO
{
    public class UserLoginRequest
    {
        public string Login { get; set; }   
        public UserLoginRequest(string login)
        {
            Login = login;
        }
    }
}
