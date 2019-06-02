using System;
using Newtonsoft.Json;
using IX.Spider;

namespace Ignite.Core.Components.Auth.Types
{
    internal class User
    {
        public int Id { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }

        public static User FromCrypto(string cryptoString)
        {
            SpiderSiph crypto = new SpiderSiph();

            try
            {
                var result = crypto.Decrypt(cryptoString, ApplicationEnv.Instance.CurrentHardware.GetOS().GetOSNumber());

                return JsonConvert.DeserializeObject<User>(result);
            }
            catch(Exception)
            {
                return null;
            }
        }

        public string ToCrypto()
        {
            SpiderSiph crypto = new SpiderSiph();

            try
            {
                return crypto.Decrypt(JsonConvert.SerializeObject(this), ApplicationEnv.Instance.CurrentHardware.GetOS().GetOSNumber());
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
