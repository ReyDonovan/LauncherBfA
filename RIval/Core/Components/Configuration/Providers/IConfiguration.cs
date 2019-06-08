using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ignite.Core.Components.Configuration.Providers
{
    public interface IConfiguration
    {
        void Initialize();
        void Add<T>(T data, T @default);
        void Append<T>(T data, bool isDefault);
        T Read<T>(bool @default);
        void Build();

        void MakeDefault<T>();
    }
}
