namespace AutoCareDiray.Shared.DTOs.UserDTO
{
    public class UserAuthorization
    {
        public string Password { get; set; }
        public string Login { get; set; }

        public UserAuthorization(string password, string login)
        {
            Password = password;
            Login = login;
        }
    }
}
