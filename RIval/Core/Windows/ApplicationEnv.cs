using IX.Composer.Architecture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Version = IX.Composer.Architecture.Version;

namespace RIval.Core
{
    public enum ApplicationStatus
    {
        Disconnected,
        Connected,
        Loading,
        Banned,
        Blocked,
        Normal
    }

    public class ApplicationEnv : Singleton<ApplicationEnv>
    {
        public ApplicationStatus Status { get; private set; }
        public Languages Language { get; private set; }
        public Version AppVersion { get; private set; }
        public Hardware CurrentHardware { get; private set; }

        public ApplicationEnv()
        {
            AppVersion = new Version("1.0.16.184");
            CurrentHardware = GetCoreComponent<Hardware>();
            Status = ApplicationStatus.Loading;
        }
        public void ApplyStatus(ApplicationStatus status)
        {
            SetLocalStatus(status);
            WindowMgr.Instance.Dispatch(status);
        }
        public void SetLocalStatus(ApplicationStatus status)
        {
            Status = status;
        }
        public void SetLocale(Languages lang)
        {
            Language = lang;
        }
        public void Restart()
        {
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }
    }
}
