using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RIval.Core.Components.Update
{
    public class UpdateFacade : Singleton<UpdateFacade>
    {
        private UpdateApiHandler ApiHandle = new UpdateApiHandler();

        public bool IsUpdateNeeded()
        {
            var result = UpdateParser.Parse(ApiHandle.GetRemoteVersionStr());
            if(result.ToString() != "0.0.0.A")
            {
                if(UpdateParser.Compare(result, ApplicationEnv.Instance.AppVersion) == CompareResult.Higher)
                {
                    return true;
                }
            }

            return false;
        }

        public void StartUpdate()
        {
            if(!File.Exists("SierraUpdater.exe"))
            {
                WebClient client = new WebClient();
                client.DownloadFileCompleted += OnDownloadCompleted;
                client.DownloadFileAsync(new Uri(ApiHandle.GetUpdaterUri()), "SierraUpdater.exe");
            }
            else
            {
                Process.Start("SierraUpdater.exe");

                Environment.Exit(0);
            }
        }

        private void OnDownloadCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if(e.Error == null)
            {
                Process.Start("SierraUpdater.exe");
            }
            else
            {
                MessageBox.Show("Во время выполнения системы обновления было выброшено исключение\nПовторите попытку позже, если проблема не решилась самостоятельно, то обратитесь к Администратору.", "Ошибка обновления", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
