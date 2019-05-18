using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RIval.Core.Components.Auth
{
    public class AuthFacade : Singleton<AuthFacade>
    {
        public bool Attempt(string user, string password)
        {
            return false;
        }
    }
}
