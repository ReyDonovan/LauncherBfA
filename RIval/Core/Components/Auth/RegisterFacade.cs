using Ignite.Core.Components.Api;
using Ignite.Core.Components.Auth.Types;
using System.Collections.Generic;
using System.Linq;

namespace Ignite.Core.Components.Auth
{
    public class RegisterFacade : Singleton<RegisterFacade>
    {
        public AuthResult Attempt(string user, string password, string question, string answer)
        {
            return ApiFacade.Instance.Builder<AuthResult>()
                .CreateRequest("https://bfa.wowlegions.ru/api/auth/signup", RequestMethod.POST, new Dictionary<string, string>()
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

            //return ApiFacade.Instance.Builder<Types.AuthResult>().CreateRequest(ApiFacade.Instance.BuildUri("api-user-register", password, user, secret)).GetResponse().First();
        }
    }
}
