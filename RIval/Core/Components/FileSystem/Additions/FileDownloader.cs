using Ignite.Core.Components.FileSystem.Types;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Ignite.Core.Components.FileSystem.Additions
{
    public class FileDownloader
    {
        public delegate void FileDownloaderProcess(string info, int percentage);
        public event FileDownloaderProcess OnDownload;

        private string Error = "";
        private Stopwatch Sw = new Stopwatch();
        private bool LastResult = true;
        private string CurrentFile = "";

        public void Subscribe(FileDownloaderProcess handler)
        {
            try
            {
                if (!OnDownload.GetInvocationList().Contains(handler))
                {
                    OnDownload += handler;
                }
            }
            catch (NullReferenceException)
            {
                OnDownload += handler;
            }
        }

        public void Unsubscribe(FileDownloaderProcess handler)
        {
            try
            {
                if (OnDownload.GetInvocationList().Contains(handler))
                {
                    OnDownload -= handler;
                }
            }
            catch (NullReferenceException)
            {
                
            }
        }

        public async Task<bool> UpdateClientAsync(FileObjectsCollection files)
        {
            return await Task.Run(() => UpdateClient(files));
        }

        public bool UpdateClient(FileObjectsCollection files)
        {
            var handler = new WebClient();
            bool result = true;

            try
            {
                foreach (var item in files)
                {
                    if (!File.Exists(item.FileName))
                    {
                        CurrentFile = item.NiceFileName;

                        if (item.NiceFileName.Length > 35)
                        {
                            CurrentFile = CurrentFile.Remove(35) + "...";
                        }

                        if (!Directory.Exists(item.FileName.Replace(item.FileName.Split('\\').Last(), "")))
                        {
                            Directory.CreateDirectory(item.FileName.Replace(item.FileName.Split('\\').Last(), ""));
                        }

                        handler.DownloadProgressChanged += OnProgressChanged_Remote;
                        handler.DownloadFileCompleted += OnDownloadComplete_Remote;

                        Sw.Start();
                        handler.DownloadFileTaskAsync(item.RemotePath, item.FileName).Wait();

                        if (LastResult != true)
                        {
                            result = false;

                            break;
                        }

                        Task.Delay(1500);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToLog(LogLevel.Error);
                Error = ex.Message;

                result = false;
            }

            return result;
        }
        public void Stop(string reason)
        {
            Error = reason;

            LastResult = false;
        }

        private void OnDownloadComplete_Remote(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            LastResult = e.Error == null;

            Sw.Stop();
        }

        private void OnProgressChanged_Remote(object sender, DownloadProgressChangedEventArgs e)
        {
            string info = $"{CurrentFile}";
            if (info.Length > 25)
            {
                info = info.Remove(25) + "...";
            }
            info += $" ({FileUtils.GetSpeed(e.BytesReceived, Sw)})";

            OnDownload(info, e.ProgressPercentage);
        }

        public string GetError()
        {
            return Error;
        }
    }
}
