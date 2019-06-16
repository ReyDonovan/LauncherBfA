using Ignite.Core.Components.FileSystem;
using Ignite.Core.Components.FileSystem.Additions;
using Ignite.Core.Components.FileSystem.Types;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Ignite.Core.Components
{
    public class FileMgr : Singleton<FileMgr>
    {
        private FileObjectsCollection Collection { get; set; }
        private FileChecker           Checker    { get; set; }
        private FileDownloader        Downloader { get; set; }

        public FileMgr()
        {
            Collection = new FileObjectsCollection();
            Checker    = new FileChecker();
            Downloader = new FileDownloader();
        }

        public bool IsWowDirectory(string path)
        {
            return File.Exists(path + "\\Wow.exe") || File.Exists(path + "\\Wow-64.exe");
        }
        public FileCrashBuilder CreateCrash()
        {
            return new FileCrashBuilder();
        }

        public bool Check()
        {
            if (Collection.Count <= 0) return false;
            else
            {
                return Checker.Check(Collection);
            }
        }
        public async Task<bool> CheckAsync()
        {
            if (Collection.Count <= 0) return false;
            else
            {
                return await Checker.CheckAsync(Collection);
            }
        }

        public bool Download()
        {
            if (Collection.Count <= 0) return false;
            else
            {
                return Downloader.UpdateClient(Collection);
            }
        }
        public async Task<bool> DownloadAsync()
        {
            if (Collection.Count <= 0) return false;
            else
            {
                return await Downloader.UpdateClientAsync(Collection);
            }
        }
        public string GetDownloaderError()
        {
            return Downloader.GetError();
        }

        public void Subscribe<T>(T handler)
        {
            if(typeof(T) == typeof(FileDownloader.FileDownloaderProcess))
            {
                Downloader.Subscribe((FileDownloader.FileDownloaderProcess)(object)handler);
            }

            if(typeof(T) == typeof(FileChecker.FileCheckerProcess))
            {
                Checker.Subscribe((FileChecker.FileCheckerProcess)(object)handler);
            }
        }

        public void Unsubscribe<T>(T handler)
        {
            if (typeof(T) == typeof(FileDownloader.FileDownloaderProcess))
            {
                Downloader.Unsubscribe((FileDownloader.FileDownloaderProcess)(object)handler);
            }

            if (typeof(T) == typeof(FileChecker.FileCheckerProcess))
            {
                Checker.Unsubscribe((FileChecker.FileCheckerProcess)(object)handler);
            }
        }

        public bool IsEnoughFreeSpace(string path)
        {
            DriveInfo drive = DriveInfo.GetDrives().First((dr) => dr.Name.Contains(path.Split(':').First()));
            if (drive != null)
            {
                return drive.AvailableFreeSpace >= ApplicationEnv.Instance.NeededGameBytes;
            }

            return false;
        }

        public Task<bool> GetManifest(bool full, int serverid, string path)
        {
            return Task.Run(() =>
            {
                try
                {
                    var res = (full)
                    ? Api.ApiFacade.Instance.Builder<FileObj>().CreateRequest(Api.ApiFacade.Instance.GetUri("api-get-clientfiles-full") + $"/{serverid}").GetResponse()
                    : Api.ApiFacade.Instance.Builder<FileObj>().CreateRequest(Api.ApiFacade.Instance.GetUri("api-get-clientfiles-mini") + $"/{serverid}").GetResponse();

                    Collection.Clear();

                    for (int i = 0; i < res.Count; i++)
                    {
                        res[i].Id = i;
                        res[i].NiceFileName = res[i].FileName;
                        res[i].FileName = path + res[i].FileName;
                        Collection.Add(res[i]);
                    }

                    return Collection.Count > 0;
                }
                catch(Exception ex)
                {
                    ex.ToLog(LogLevel.Error);

                    return false;
                }
            });
        }

        public Task<string> CacheImage(string path, string url)
        {
            return Task.Run(() =>
            {
                System.Net.WebClient client = new System.Net.WebClient();

                var localname = url.Split('/').Last();
                client.DownloadFile(url, $"{path}\\{localname}");

                return path + localname;
            });
        }
    }
}
