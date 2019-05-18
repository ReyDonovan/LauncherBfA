using RIval.Core.Components.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RIval.Core.Components.Auth
{
    public class RegisterFacade : Singleton<RegisterFacade>
    {
        public Types.AuthResult Attempt(string user, string password, string secret)
        {
            return ApiFacade.Instance.Builder<Types.AuthResult>().CreateRequest(ApiFacade.Instance.BuildUri("api-user-register", password, user, secret)).GetResponse().First();
        }
    }
}
