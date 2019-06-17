using IX.Composer.Architecture;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Windows;
using Version = IX.Composer.Architecture.Version;

namespace Ignite.Core
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

        private Dictionary<int, string> Servers = new Dictionary<int, string>();

        public long NeededGameBytes = 64424509440;

        public ApplicationEnv()
        {
            AppVersion = new Version("1.3.4.5447");
            CurrentHardware = GetCoreComponent<Hardware>();
            Status = ApplicationStatus.Loading;

            BootIps();
        }

        private void BootIps()
        {
            Servers.Add(1, "logon.wowignite.ru");
            Servers.Add(2, "logon.wowignite.ru");
        }
        public string GetPortal(int serverId)
        {
            if(Servers.ContainsKey(serverId))
            {
                return Servers[serverId];
            }

            return "127.0.0.1";
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

            WindowMgr.Instance.AppendLanguages();
        }
        public void Restart()
        {
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);

            try
            {
                Application.Current.Shutdown();
            }
            catch(Exception)
            {
                Environment.Exit(0);
            }
        }

        public bool IsUserAdministrator()
        {
            bool isAdmin = false;
            try
            {
                WindowsIdentity user = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new WindowsPrincipal(user);
                isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            
            catch (UnauthorizedAccessException) { }
            catch (Exception) { }

            return isAdmin;
        }
    }
}
