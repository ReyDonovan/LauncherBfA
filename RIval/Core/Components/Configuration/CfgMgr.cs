using Ignite.Core.Components.Configuration;
using Ignite.Core.Components.Configuration.Providers;

namespace Ignite.Core.Components
{
    public class CfgMgr : Singleton<CfgMgr>
    {
        private ConfigurationProvider Provider = new ConfigurationProvider();

        public CfgMgr()
        {
            CreateDefault();
        }
        
        public void Create(IConfiguration provider)
        {
            Provider.AppendProvider(provider);
        }

        public void CreateDefault()
        {
            Provider.AppendProvider(JsonConfiguration.Prototype());
        }

        public ConfigurationProvider GetProvider() => Provider;
    }
}
