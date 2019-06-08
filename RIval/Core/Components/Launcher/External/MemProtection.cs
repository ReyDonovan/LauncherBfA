using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ignite.Core.Components.Launcher.External
{
    public enum MemProtection
    {
        // Windows
        NoAccess = 0x001,
        ReadOnly = 0x002,
        ReadWrite = 0x004,
        WriteCopy = 0x008,
        Execute = 0x010,
        ExecuteRead = 0x020,
        ExecuteReadWrite = 0x040,
        ExecuteWriteCopy = 0x080,
        Guard = 0x100,
        NoCache = 0x200,
        WriteCombine = 0x400,
    }
}
