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
                            ApiFacade.Instance.GetUri("api-user-login"),
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
                    CreateUser();
                }
                catch(ArgumentNullException)
                {
                    LogAsync(this, $"Attempted login when token has been null value", LogLevel.Error);

                    CurrentUser = null;
                }
                catch(Exception ex)
                {
                    LogAsync(this, $"Attempted login by token remember with error: {ex.Message}. Data: {GetToken()}", LogLevel.Error);

                    CurrentUser = null;
                }

                return CurrentUser;
            });
        }

        public void Logout()
        {
            string path = "cache";

            if (File.Exists(path + $"\\crd.sflow"))
            {
                File.Delete(path + $"\\crd.sflow");
            }

            ApplicationEnv.Instance.Restart();
        }

        private void CreateUser()
        {
            try
            {
                CurrentUser = ApiFacade.Instance
                    .Builder<User>()
                    .AddHeader("Authorization", $"Bearer {GetToken()}")
                    .CreateRequest(ApiFacade.Instance.GetUri("api-user-getdata"))
                    .GetResponse()
                    .First();
            }
            catch(Exception ex)
            {
                LogAsync(this, $"Failed after getting user data from api: {ex.Message}. Data: {GetToken()}", LogLevel.Error);
            }
        }
        
        private string GetToken()
        {
            string path = "cache";

            if (File.Exists(path + $"\\crd.sflow"))
            {
                string result = File.ReadAllText(path + $"\\crd.sflow");
                if(result != null || result != "" || result != string.Empty)
                {
                    return result;
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
            string path = "cache";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            if (!File.Exists(path + $"\\crd.sflow"))
            {
                File.Create(path + $"\\crd.sflow");
            }

            File.WriteAllText(path + $"\\crd.sflow", CurrentUser.Token);
        }
        public void Save(string token)
        {
            CurrentUser = new User
            {
                Token = token
            };

            Save();
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

                    return (T)(object)CurrentUser;
                }
                catch (Exception)
                {
                    return (T)(object)null;
                }
            }
            else
                throw new InvalidCastException();
        }

        public async override Task<T> DoAsync<T>(params object[] data)
        {
            if (typeof(T) == typeof(AuthResult))
            {
                if (data.Count() < 2) throw new InvalidDataException();
                var data_str = data.Reinterpret<string>();

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
