using Ignite.Core.Components.Api;
using Ignite.Core.Components.Auth.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;

namespace Ignite.Core.Components.Auth
{
    public class AuthFacade : Facade
    {
        internal User CurrentUser { get; private set; } = null;

        public AuthFacade()
        {
            
        }

        private Task<AuthResult> Attempt(string user, string password)
        {
            return Task.Run(() =>
            {
                try
                {
                    return ApiFacade.Instance.Builder<AuthResult>()
                        .CreateRequest(
                            //ApiFacade.Instance.GetUri("api-user-login"),
                            "https://bfa.wowlegions.ru/api/login",
                            RequestMethod.POST, new Dictionary<string, string>()
                            {
                                ["email"] = user,
                                ["password"] = password
                            })
                        .GetResponse()
                        .First();
                }
                catch (Exception ex)
                {
                    LogAsync(this, $"Attempted login with error: {ex.Message}. Data: {user}", LogLevel.Error);

                    return new AuthResult() { Code = 0, Message = "api_server_error", Token = "" };
                }
            });
        }
        private Task<User> AttemptToken()
        {
            return Task.Run(() =>
            {
                try
                {
                    if (GetToken() == null) throw new ArgumentNullException();

                    var result = ApiFacade.Instance.Builder<AuthResult>()
                    .CreateRequest(
                        //ApiFacade.Instance.GetUri("api-user-remember-token"),
                        "https://bfa.wowlegions.ru/oauth/login",
                        RequestMethod.POST,
                        new Dictionary<string, string>()
                        {
                            ["token"] = GetToken()
                        })
                    .GetResponse()
                    .First();

                    if (result.Code == AuthResultEnum.Ok)
                    {
                        CreateUser();
                    }
                }
                catch(ArgumentNullException)
                {
                    LogAsync(this, $"Attempted login when token has been null value", LogLevel.Error);

                    CurrentUser = null;
                }
                catch(Exception ex)
                {
                    LogAsync(this, $"Attempted login by token remember  with error: {ex.Message}. Data: {GetToken()}", LogLevel.Error);

                    CurrentUser = null;
                }

                return CurrentUser;
            });
        }

        private void CreateUser()
        {
            try
            {
                CurrentUser = ApiFacade.Instance.Builder<User>().CreateRequest("https://bfa.wowlegions.ru/api/user").GetResponse().First();
            }
            catch(Exception ex)
            {
                LogAsync(this, $"Failed after getting user data from api: {ex.Message}. Data: {GetToken()}", LogLevel.Error);
            }

            Save();
        }
        
        private string GetToken()
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Ignite");

            if (File.Exists(path + $"\\crd.sflow"))
            {
                string result = File.ReadAllText(path + $"\\crd.sflow");
                if(result != null || result != "" || result != string.Empty)
                {
                    return User.GetToken(result);
                }
            }
            else
            {
                if(!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                if(!File.Exists(path + $"\\crd.sflow"))
                {
                    File.Create(path + $"\\crd.sflow");
                }
            }

            return null;
        }

        public void Save()
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Ignite");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            if (!File.Exists(path + $"\\crd.sflow"))
            {
                File.Create(path + $"\\crd.sflow");
            }

            File.WriteAllText(path + $"\\crd.sflow", CurrentUser.SaveToken());
        }

        public override T Do<T>(params object[] data)
        {
            if (typeof(T) == typeof(AuthResult))
            {
                if (data.Count() < 2) throw new InvalidDataException();
                var data_str = data.Reinterpret<string>();

                try
                {
                    var result = Attempt(data_str[0], data_str[1]);
                    result.Wait();

                    return (T)(object)result.GetAwaiter().GetResult();
                }
                catch(Exception)
                {
                    return (T)(object)AuthResult.BuildServerError();
                }
            }
            else if (typeof(T) == typeof(User))
            {
                try
                {
                    var result = AttemptToken();
                    result.Wait();

                    return (T)(object)result.GetAwaiter().GetResult();
                }
                catch (Exception)
                {
                    return (T)(object)CurrentUser;
                }
            }
            else
                throw new InvalidCastException();
        }

        public async override Task<T> DoAsync<T>(params object[] data)
        {
            if (data.Count() <= 0) throw new ArgumentNullException();
            var data_str = data.Reinterpret<string>();

            if (typeof(T) == typeof(AuthResult))
            {
                if (data.Count() < 2) throw new InvalidDataException();

                try
                {
                    return (T)(object)await Attempt((string)data[0], (string)data[1]);
                }
                catch (Exception)
                {
                    return (T)(object)AuthResult.BuildServerError();
                }
            }
            else if (typeof(T) == typeof(User))
            {
                if (data.Count() < 1) throw new InvalidDataException();

                try
                {
                    return (T)(object)await AttemptToken();
                }
                catch (Exception)
                {
                    return (T)(object)CurrentUser;
                }
            }
            else
                throw new InvalidCastException();
        }

        public override Facade GetFacadeAccessor()
        {
            return this;
        }
    }
}
