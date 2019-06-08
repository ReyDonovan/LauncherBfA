using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ignite.Core.Components.Launcher.External
{
    public enum MemType : uint
    {
        Private = 0x20000,
        Mapped = 0x40000,
        Image = 0x1000000
    }
}
