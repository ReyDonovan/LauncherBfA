using Ignite.Core.Components.FileSystem;
using Ignite.Core.Components.FileSystem.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Ignite.Core.Components
{
    public class FileMgr : Singleton<FileMgr>
    {
        private FileObjectsCollection Collection = new FileObjectsCollection();

        public delegate void FileCheckStarted(string fn, int percent);
        public delegate void FileCheckStopped(string fn, bool result);
        public delegate void FileDownloadStarted(string fn);
        public delegate void FileDownloadStopped(string fn, bool result);
        public delegate void FileDownloadProcess(string info, int percentage, int currentFilePercentage);
        public delegate void AllDownloadsStopped(bool result);

        public event FileCheckStarted OnCheckStarted;
        public event FileCheckStopped OnCheckStopped;
        public event FileDownloadStarted OnDownloadStarted;
        public event FileDownloadStopped OnDownloadStopped;
        public event FileDownloadProcess OnDownloadProcess;
        public event AllDownloadsStopped OnStoppedProcesses;

        private int CurrentDownloadedFile = 0;
        private int CurrentFileChecked = 0;
        private string CurrentDownloading = "";
        private bool IsWrongFiles = true;
        private Stopwatch Sw = new Stopwatch();

        public bool IsWowDirectory(string path)
        {
            return File.Exists(path + "\\Wow.exe") || File.Exists(path + "\\Wow-64.exe");
        }

        public FileCrashBuilder CreateCrash()
        {
            return new FileCrashBuilder();
        }

        public void StartCheck()
        {
            Task.Run(() =>
            {
                string current = "";
                CurrentFileChecked = 0;

                try
                {
                    foreach (var item in Collection)
                    {
                        current = item.NiceFileName;
                        OnCheckStarted(item.NiceFileName, ((CurrentFileChecked * 100) / Collection.Count));

                        if (!File.Exists(item.FileName))
                        {
                            IsWrongFiles = true;
                        }
                        else
                        {
                            if(!CompareHashRaw(item.FileName, item.Hash))
                            {
                                IsWrongFiles = true;

                                File.Delete(item.FileName);
                            }
                        }

                        CurrentFileChecked++;
                    }

                    OnCheckStopped(current, !IsWrongFiles);
                }
                catch(Exception ex)
                {
                    ex.ToLog(LogLevel.Error);

                    OnCheckStopped(current, false);
                }
            });
        }
        public void StartUpdate(string path, int server = 1)
        {
            Task.Run(() =>
            {
                DriveInfo drive = DriveInfo.GetDrives().First((dr) => dr.Name.Contains(path.Split(':').First()));
                if(drive != null)
                {
                    if(drive.AvailableFreeSpace <= 64424509440)
                    {
                        MessageBox.Show(
                            string.Format("Not enough free disk space: {0}\nFree space: {1}\nNeeded: {2}\n\nIt is not possible to continue with the installation.", 
                                drive.Name, 
                                FormatByte(drive.AvailableFreeSpace), 
                                FormatByte(ApplicationEnv.Instance.NeededGameBytes)), 
                            LanguageMgr.Instance.ValueOf("Error_FreeSpaceUnavailable_Title"), 
                            MessageBoxButton.OK, 
                            MessageBoxImage.Error);

                        return;
                    }
                }

                var handler = new WebClient();

                try
                {
                    foreach (var item in Collection)
                    {
                        if(!File.Exists(item.FileName))
                        {
                            CurrentDownloading = item.NiceFileName;

                            string info = item.NiceFileName;
                            if (info.Length > 35)
                            {
                                info = info.Remove(35) + "...";
                            }

                            OnDownloadStarted(info);

                            handler.DownloadProgressChanged -= Handler_DownloadProgressChanged;
                            handler.DownloadProgressChanged += Handler_DownloadProgressChanged;
                            handler.DownloadFileCompleted -= Handler_DownloadFileCompleted;
                            handler.DownloadFileCompleted += Handler_DownloadFileCompleted;

                            if(!Directory.Exists(item.FileName.Replace(item.FileName.Split('\\').Last(), "")))
                            {
                                Directory.CreateDirectory(item.FileName.Replace(item.FileName.Split('\\').Last(), ""));
                            }

                            Sw.Start();
                            handler.DownloadFileTaskAsync(item.RemotePath, item.FileName).Wait();

                            CurrentDownloadedFile++;

                            Task.Delay(1500);
                        }
                    }

                    IsWrongFiles = false;
                    OnStoppedProcesses(true);
                }
                catch(Exception ex)
                {
                    Logger.Instance.WriteLine(ex.ToString() + $" File: {CurrentDownloading}", LogLevel.Error);

                    OnStoppedProcesses(false);
                }
            });
        }

        private void Handler_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if(e.Error != null)
            {
                e.Error.ToLog(LogLevel.Error);

                OnStoppedProcesses(false);
            }

            Sw.Stop();
        }

        private void Handler_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            string info = $"{CurrentDownloading}";
            if(info.Length > 25)
            {
                info = info.Remove(25) + "...";
            }
            info += $" ({GetSpeed(e.BytesReceived)})";

            OnDownloadProcess(info, ((CurrentDownloadedFile * 100) / Collection.Count), e.ProgressPercentage);
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

                    return true;
                }
                catch(Exception ex)
                {
                    ex.ToLog(LogLevel.Error);

                    return false;
                }
            });
        }

        public bool CompareHash(string h1, string h2)
        {
            return h1.ToLower() == h2.ToLower();
        }

        public bool CompareHashRaw(string fullpath, string hash)
        {
            using (var stream = File.OpenRead(fullpath))
            {
                return CompareHash(GetHash<MD5>(stream), hash);
            }
        }

        private string GetHash<T>(Stream stream) where T : HashAlgorithm
        {
            StringBuilder sb = new StringBuilder();

            MethodInfo create = typeof(T).GetMethod("Create", new Type[] { });
            using (T crypt = (T)create.Invoke(null, null))
            {
                byte[] hashBytes = crypt.ComputeHash(stream);
                foreach (byte bt in hashBytes)
                {
                    sb.Append(bt.ToString("x2"));
                }
            }

            return sb.ToString();
        }

        public void CreateDirectory(string path)
        {
            if(Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        private string FormatByte(long bytes)
        {
            int i;
            double dblSByte = bytes;
            for (i = 0; i < 5 && bytes >= 1024; i++, bytes /= 1024)
            {
                dblSByte = bytes / 1024.0;
            }

            return String.Format("{0:0.##} {1}", dblSByte, GetSuffix(i));
        }
        private string GetSpeed(long bytes)
        {
            int i;
            double dblSbyte = bytes;

            for (i = 0; i < 5 && bytes >= 1024; i++, bytes /= 1024)
            {
                dblSbyte = bytes / 1024.0;
            }

            return $"{(dblSbyte / Sw.Elapsed.TotalSeconds).ToString("0.00")} {GetSuffix(i)}/{LanguageMgr.Instance.ValueOf("Seconds_Short")}";
        }

        private string GetSuffix(int index)
        {
            string[] suffix =
            {
                "Bytes", "Kb", "Mb", "Gb", "Tb"
            };

            return LanguageMgr.Instance.ValueOf(suffix[index]);
        }
    }
}
