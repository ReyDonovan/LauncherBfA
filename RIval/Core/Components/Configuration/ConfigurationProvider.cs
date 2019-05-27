using Ignite.Core.Components.Configuration.Providers;

namespace Ignite.Core.Components.Configuration
{
    public class ConfigurationProvider
    {
        private IConfiguration CurrentProvider;

        public void AppendProvider(IConfiguration config)
        {
            CurrentProvider = config;
        }

        public T Read<T>(bool def)
        {
            return CurrentProvider.Read<T>(def);
        }
        public void Write<T>(T data, bool def)
        {
            CurrentProvider.Append(data, def);
        }

        public ConfigurationProvider Add<T>(T data, T def)
        {
            CurrentProvider.Add(data, def);

            return this;
        }

        public void Build()
        {
            CurrentProvider.Build();
        }
    }
}
