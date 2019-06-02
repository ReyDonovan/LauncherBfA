using Ignite.Core.Components.Api;
using Ignite.Core.Components.Auth.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;

namespace Ignite.Core.Components.Auth
{
    public class AuthFacade : Singleton<AuthFacade>
    {
        internal User CurrentUser { get; private set; } = null;

        public AuthFacade()
        {
            BuildUser();
        }

        public AuthResult Attempt(string user, string password)
        {
            return ApiFacade.Instance.Builder<AuthResult>()
                .CreateRequest(
                    //ApiFacade.Instance.GetUri("api-user-login"),
                    "https://bfa.wowlegions.ru/api/auth/login",
                    RequestMethod.POST, new Dictionary<string, string>()
                    {
                        ["email"] = user,
                        ["password"] = password
                    })
                .GetResponse()
                .First();
        }

        public AuthResult AttemptToken()
        {
            return ApiFacade.Instance.Builder<AuthResult>()
                .CreateRequest(
                    ApiFacade.Instance.GetUri("api-user-remember-token"),
                    RequestMethod.POST,
                    new Dictionary<string, string>()
                    {
                        ["email"] = CurrentUser.Email,
                        ["token"] = CurrentUser.Token
                    })
                .GetResponse()
                .First();
        }

        public void CreateUser()
        {
            //TODO: 
            CurrentUser = ApiFacade.Instance.Builder<User>().CreateRequest("http://bfa.wowlegions.ru/api/user").GetResponse().First();
            
            Save();
        }

        public bool IsRemembered()
        {
            return CurrentUser != null;
        }
        
        private void BuildUser()
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Ignite");

            if (File.Exists(path + $"\\crd.sflow"))
            {
                string result = File.ReadAllText(path + $"\\crd.sflow");
                if(result != null || result != "" || result != string.Empty)
                {
                    CurrentUser = User.FromCrypto(result);
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
        }

        private void Save()
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

            File.WriteAllText(path + $"\\crd.sflow", CurrentUser.ToCrypto());
        }
    }
}
