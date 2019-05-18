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
            return default;
        }
    }
}
