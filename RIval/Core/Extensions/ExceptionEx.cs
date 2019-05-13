using RIval.Core.Components;
using System;

namespace RIval.Core
{
    public static class ExceptionEx
    {
        public static void ToLog(this Exception ex, LogLevel level)
        {
            Logger.Instance.WriteLine(ex.Message, level);
        }
    }
}
