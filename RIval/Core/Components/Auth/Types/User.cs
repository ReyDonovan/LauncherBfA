using System;
using Newtonsoft.Json;
using IX.Spider;

namespace Ignite.Core.Components.Auth.Types
{
    public class User
    {
        public int Id { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public string AvatarRemote { get; set; }

        public static string GetToken(string cryptoString)
        {
            SpiderSiph crypto = new SpiderSiph();

            try
            {
                return crypto.Decrypt(cryptoString, ApplicationEnv.Instance.CurrentHardware.GetOS().GetOSNumber());
            }
            catch(Exception)
            {
                return null;
            }
        }

        public string SaveToken()
        {
            SpiderSiph crypto = new SpiderSiph();

            try
            {
                return crypto.Decrypt(Token, ApplicationEnv.Instance.CurrentHardware.GetOS().GetOSNumber());
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
