using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ignite.Core.Components.Auth.Types
{
    public class AuthResult
    {
        public AuthResultEnum Code    { get; set; }
        public string         Message { get; set; }
        public string         Token   { get; set; }

        public static AuthResult BuildServerError()
        {
            return new AuthResult()
            {
                Code = AuthResultEnum.Error,
                Message = "api_server_error",
                Token = null
            };
        }
    }

    public enum AuthResultEnum : int
    {
        Invalid = 0,
        Ok = 1,
        Error = 2
    }
}
