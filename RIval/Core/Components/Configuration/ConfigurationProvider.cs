using Ignite.Core.Components.Configuration.Providers;
using System;

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
        public void Write<T>(Action<T> act, bool def)
        {
            var setting = CurrentProvider.Read<T>(def);
            if(setting != null)
            {
                act(setting);

                Write(setting, def);
            }
            else
                throw null;
        }

        public void Write<T>(T data, bool def)
        {
            CurrentProvider.Append(data, def);
        }

        public void Restore<T>()
        {
            CurrentProvider.MakeDefault<T>();
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
