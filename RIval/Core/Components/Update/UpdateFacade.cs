using Ignite.Core.Components.Message;
using IX.Composer.Architecture;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using Version = IX.Composer.Architecture.Version;

namespace Ignite.Core.Components.Update
{
    public class UpdateFacade : Singleton<UpdateFacade>
    {
        private UpdateApiHandler ApiHandle = new UpdateApiHandler();

        public bool IsUpdateNeeded()
        {
            try
            {
                var response = ApiHandle.GetRemoteVersionStr();
                if (Version.TryParse(response, out var ver))
                {
                    if (ApplicationEnv.Instance.AppVersion.ToString() == ver.ToString())
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                    return true;
            }
            catch(Exception)
            {
                MessageBoxMgr.Instance.ShowCriticalError(LanguageMgr.Instance.ValueOf("Updater_UpdateErrorMB_Title"), LanguageMgr.Instance.ValueOf("Updater_UpdateErrorMB_Desc"));

                return false;
            }
        }

        public void StartUpdate()
        {
            if(!File.Exists("IgniteUpdater.exe"))
            {
                WebClient client = new WebClient();
                client.DownloadFileCompleted += OnDownloadCompleted;

                client.DownloadFileAsync(new Uri(ApiHandle.GetUpdaterUri()), "IgniteUpdater.exe");
            }
            else
            {
                Process.Start("IgniteUpdater.exe");
                Environment.Exit(0);
            }
        }

        private void OnDownloadCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if(e.Error == null)
            {
                Process.Start("IgniteUpdater.exe");
                Environment.Exit(0);
            }
            else
            {
                e.Error.ToLog(LogLevel.Error);
            }
        }
    }
}
