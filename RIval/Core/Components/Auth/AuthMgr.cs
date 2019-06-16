using Ignite.Core.Components.Auth.Types;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Ignite.Core.Components.Auth
{
    public class AuthMgr : Singleton<AuthMgr>
    {
        private AuthFacade     Auth     { get; set; } = new AuthFacade();
        private RegisterFacade Signup { get; set; } = new RegisterFacade();

        public async Task<AuthResult> LoginAsync(string user, string password)
        {
            return await Auth.DoAsync<AuthResult>(user, password);
        }

        public AuthResult Login(string user, string password)
        {
            return Auth.Do<AuthResult>(user, password);
        }

        public async Task<AuthResult> RegisterAsync(string name, string user, string password, string question, string answer)
        {
            return await Signup.DoAsync<AuthResult>(name, user, password, question, answer);
        }

        public AuthResult Register(string name, string user, string password, string question, string answer)
        {
            return Signup.Do<AuthResult>(name, user, password, question, answer);
        }

        public void Logout()
        {
            Auth.Logout();
        }

        public User LoadUser()
        {
            return Auth.Do<User>();
        }

        public async Task<User> LoadUserAsync()
        {
            return await Auth.DoAsync<User>();
        }

        public async void SaveUserAsync()
        {
            await Task.Run(() => { Auth.Save(); });
        }
        public async void SaveUserAsync(string token)
        {
            await Task.Run(() => { Auth.Save(token); });
        }
        public void SaveUser()
        {
            Auth.Save();
        }
        public void SaveUser(string token)
        {
            Auth.Save(token);
        }

        public async Task<string> GetUserAvatar()
        {
            var user = GetUser();
            if (!string.IsNullOrEmpty(user.AvatarRemote))
            {
                var avatar = user.AvatarRemote.Split('/').Last();
                if (!string.IsNullOrEmpty(avatar))
                {
                    if (!File.Exists("cache\\auth\\" + avatar))
                    {
                        return await FileMgr.Instance.CacheImage("cache\\auth", user.AvatarRemote);
                    }
                    else
                    {
                        return await Task.Run(() =>
                        {
                            return "cache\\auth\\" + avatar;
                        });
                    }
                }
                else
                    return null;
            }
            else
                return null;
        }

        public T GetFacadeAccessor<T>() where T : Facade
        {
            if (typeof(T) == typeof(AuthFacade))
                return (T)Auth.GetFacadeAccessor();
            else if (typeof(T) == typeof(RegisterFacade))
                return (T)Signup.GetFacadeAccessor();
            else
                return (T)(object)null;
        }

        public User GetUser()
        {
            return Auth.CurrentUser ?? null;
        }
    }
}
