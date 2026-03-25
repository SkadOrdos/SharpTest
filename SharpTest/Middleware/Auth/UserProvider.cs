using System.Text.Json.Serialization;

namespace WebSharp.Auth
{
    public interface IUserProvider
    {
        bool VerifyUser(string login, string pwdHash);
    }

    public class UserInfo
    {
        public string Login { get; set; }

        /// <summary>
        /// SHA256(String.Concat(login, ":", password))
        /// </summary>
        public string Password { get; set; }

        public UserInfo() { }
    }


    public class UserProvider : IUserProvider
    {
        public bool VerifyUser(string login, string pwdHash)
        {
            UserInfo[] users;
            try
            {
                users = System.Text.Json.JsonSerializer.Deserialize<UserInfo[]>(File.OpenText("users.json").ReadToEnd());
            }
            catch (Exception ex)
            {
                return false;
            }

            return users.Any(u => String.Equals(u.Login, login, StringComparison.OrdinalIgnoreCase) &&
                String.Equals(u.Password, pwdHash, StringComparison.OrdinalIgnoreCase));
        }
    }
}
