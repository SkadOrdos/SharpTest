using System.Text.Json.Serialization;

namespace WebSharp.Auth
{
    public interface IUserProvider
    {
        bool VerifyUser(string login, string password);
    }

    public class UserInfo
    {
        public string Login { get; set; }

        /// <summary>
        /// MD5(String.Concat(login, ":", password))
        /// </summary>
        public string Password { get; set; }

        public UserInfo() { }
    }


    public class UserProvider : IUserProvider
    {
        public UserProvider()
        {

        }

        public bool VerifyUser(string login, string password)
        {
            string hashedPassword = Crypto.GetSHA256(String.Concat(login, ":", password));
            UserInfo[] users;
            try
            {
                users = System.Text.Json.JsonSerializer.Deserialize<UserInfo[]>(File.OpenText("users.json").ReadToEnd());
            }
            catch (Exception ex)
            {
                return false;
            }

            return users.Any(u => String.Equals(u.Login, login, StringComparison.OrdinalIgnoreCase) && String.Equals(u.Password, hashedPassword, StringComparison.OrdinalIgnoreCase));
        }
    }
}
