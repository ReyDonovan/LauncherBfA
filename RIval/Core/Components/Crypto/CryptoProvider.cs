using IX.Spider;

namespace Ignite.Core.Components.Crypto
{
    public class CryptoProvider : Singleton<CryptoProvider>
    {
        private SpiderSiph Cryptor { get; set; }

        public CryptoProvider()
        {
            Cryptor = new SpiderSiph();
        }

        public string Encrypt(string data, string key)
        {
            return Cryptor.Encrypt(data, key);
        }

        public string Decrypt(string data, string key)
        {
            return Cryptor.Decrypt(data, key);
        }
    }
}
