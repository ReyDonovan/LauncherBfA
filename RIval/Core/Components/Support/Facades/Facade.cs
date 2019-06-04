using Ignite.Core.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ignite.Core
{
    public abstract class Facade
    {
        public async void LogAsync(Facade obj, string log, LogLevel level)
        {
            await Task.Run(() =>
            {
                Logger.Instance.WriteLine($"[{obj.GetType().Name}]: {log}", level);
            });
        }
        public abstract T Do<T>(params object[] @params);
        public abstract Task<T> DoAsync<T>(params object[] @params);
        public abstract Facade GetFacadeAccessor();
    }
}
