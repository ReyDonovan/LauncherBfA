using Ignite.Core.Components.Api;
using Ignite.Core.Components.Auth.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Ignite.Core.Components.Auth
{
    public class RegisterFacade : Facade
    {
        public override T Do<T>(params object[] @params)
        {
            if (@params.Count() < 4) throw new InvalidDataException();
            var data = @params.Reinterpret<string>();
            var result = Attempt(data[0], data[1], data[2], data[3]);
            result.Wait();

            return (T)(object)result.GetAwaiter().GetResult();
        }

        public async override Task<T> DoAsync<T>(params object[] @params)
        {
            if (@params.Count() < 4) throw new InvalidDataException();
            var data = @params.Reinterpret<string>();

            return (T)(object)await AttemptAsync(data[0], data[1], data[2], data[3]);
        }

        public override Facade GetFacadeAccessor()
        {
            return this;
        }

        private async Task<AuthResult> AttemptAsync(string user, string password, string question, string answer)
        {
            return await Attempt(user, password, question, answer);
        }

        private Task<AuthResult> Attempt(string user, string password, string question, string answer)
        {
            return Task.Run(() =>
            {
                try
                {
                    return ApiFacade.Instance.Builder<AuthResult>()
                        .CreateRequest("https://bfa.wowlegions.ru/api/signup", RequestMethod.POST, new Dictionary<string, string>()
                        {
                            ["name"] = user.Split('@')[0],
                            ["email"] = user,
                            ["password"] = password,
                            ["question"] = question,
                            ["answer"] = answer,
                            ["receive"] = "no",
                        })
                        .GetResponse()
                        .First();
                }
                catch (Exception ex)
                {
                    LogAsync(this, $"Error while creating user by api: {ex.Message}. Data: [{user}:{password}][{question}:{answer}]", LogLevel.Error);

                    return new AuthResult() { Code = 0, Message = "api_server_error", Token = "" };
                }
            });
        }
    }
}
