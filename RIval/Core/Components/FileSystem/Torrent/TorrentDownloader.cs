using System;
using System.Linq;
using System.Threading.Tasks;

/**
 * Mono torrent namespaces
 */
using MonoTorrent.BEncoding;
using MonoTorrent.Client;
using MonoTorrent.Common;
using MonoTorrent.Client.Tracker;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Net;

namespace Ignite.Core.Components.FileSystem.Torrent
{
    public delegate void OnPeerAdded(object sender, PeersAddedEventArgs e);
    public delegate void OnTorrentChangeState(TorrentData data);
    public delegate void OnDownloadStopped();

    public class TorrentDownloader
    {
        public TorrentData Data { get; private set; }
        public TorrentDownloaderSettings Settings { get; private set; }
        public TorrentExternal External   { get; private set; }

        public event OnPeerAdded OnPeerAdd;
        public event OnTorrentChangeState OnChangeState;
        public event OnDownloadStopped DownloadStopped;

        private Stopwatch Watcher { get; set; } = new Stopwatch();

        public void Boot(TorrentDownloaderSettings settings)
        {
            Settings = settings;
            External = new TorrentExternal(new EngineSettings(Settings.DownloadPath, 12999), settings.CachePath + "\\nodesDht");

            AppDomain.CurrentDomain.ProcessExit += (e, args) =>
            {
                Stop();
            };
        }

        public void SubscribeDownload(OnDownloadStopped handler)
        {
            try
            {
                if (!OnPeerAdd.GetInvocationList().Contains(handler))
                {
                    DownloadStopped += handler;
                }
            }
            catch (NullReferenceException)
            {
                DownloadStopped += handler;
            }
        }
        public void UnsubscribeDownload(OnDownloadStopped handler)
        {
            try
            {
                if (!OnChangeState.GetInvocationList().Contains(handler))
                {
                    DownloadStopped -= handler;
                }
            }
            catch (NullReferenceException) { }
        }
        public void SubscribePeer(OnPeerAdded handler)
        {
            try
            {
                if (!OnPeerAdd.GetInvocationList().Contains(handler))
                {
                    OnPeerAdd += handler;
                }
            }
            catch (NullReferenceException)
            {
                OnPeerAdd += handler;
            }
        }
        public void SubscribeState(OnTorrentChangeState handler)
        {
            try
            {
                if (!OnChangeState.GetInvocationList().Contains(handler))
                {
                    OnChangeState += handler;
                }
            }
            catch (NullReferenceException)
            {
                OnChangeState += handler;
            }
        }
        public void UnsubscribeState(OnTorrentChangeState handler)
        {
            try
            {
                if (!OnChangeState.GetInvocationList().Contains(handler))
                {
                    OnChangeState += handler;
                }
            }
            catch (NullReferenceException) { } 
        }
        public void UnsubscribePeer(OnPeerAdded handler)
        {
            try
            {
                if (OnPeerAdd.GetInvocationList().Contains(handler))
                {
                    OnPeerAdd -= handler;
                }
            }
            catch (NullReferenceException) { }
        }

        public async Task<bool> DownloadAsync()
        {
            return await Task.Run(() =>
            {
                return Download();
            });
        }

        public bool Download()
        {
            try
            {
                string torrent = DownloadTorrentFile(Settings.ServerId);

                if (torrent != null)
                {
                    MonoTorrent.Common.Torrent decoded = null;

                    try
                    {
                        decoded = MonoTorrent.Common.Torrent.Load(torrent);
                    }
                    catch (Exception decEx)
                    {
                        decEx.ToLog(LogLevel.Error);

                        Stop();

                        return false;
                    }

                    External.AppendManager(decoded, Settings.DownloadPath, new TorrentSettings(5, 100, Settings.MaxDownloadSpeed, Settings.MaxUploadSpeed));
                    if (External.TryGetFastResume(Settings.GetResumeFile()))
                    {
                        if (External.FastResume.ContainsKey(decoded.InfoHash.ToHex()))
                        {
                            External.Manager.LoadFastResume(new FastResume((BEncodedDictionary)External.FastResume[decoded.InfoHash.ToHex()]));
                        }
                    }

                    External.Engine.Register(External.Manager);
                    External.Manager.PeersFound += OnPeersFounded;

                    External.Manager.StartAsync().Wait();

                    var thread = new Thread(() =>
                    {
                        while (External.Manager.State != TorrentState.Stopped)
                        {

                            var data = new TorrentData();
                            data.Build(
                                FileUtils.FormatByte(External.Engine.TotalDownloadSpeed),
                                FileUtils.FormatByte(External.Engine.DiskManager.TotalRead),
                                FileUtils.FormatByte(External.Engine.DiskManager.TotalWritten),
                                FileUtils.FormatByte(External.Engine.DiskManager.WriteRate),
                                (int)External.Manager.Progress);

                            OnChangeState?.Invoke(data);

                            if (data.Progress == 100) break;

                            Thread.Sleep(500);
                        }

                        DownloadStopped();
                    });

                    thread.Start();

                    return true;
                }
            }
            catch(Exception ex)
            {
                ex.ToLog(LogLevel.Error);

                return false;
            }

            return false;
        }

        private void OnPeersFounded(object sender, PeersAddedEventArgs e)
        {
            OnPeerAdd?.Invoke(sender, e);
        }

        private string DownloadTorrentFile(int serverId)
        {
            if (!Directory.Exists("cache\\fs_tr"))
                Directory.CreateDirectory("cache\\fs_tr");

            WebClient client = new WebClient();
            client.DownloadFile(new Uri($"http://wowignite.ru/public/cdn/full/{serverId}/{serverId}.torrent"), Settings.CachePath + $"\\{serverId}.torrent");

            return Settings.CachePath + $"\\{serverId}.torrent";
        }

        public void Stop()
        {
            try
            {
                BEncodedDictionary fastResume = new BEncodedDictionary();

                External.Manager.StopAsync();
                fastResume.Add(External.Manager.Torrent.InfoHash.ToHex(), External.Manager.SaveFastResume().Encode());

                File.WriteAllBytes(Settings.GetResumeFile(), fastResume.Encode());
                External.Engine.Dispose();
            }
            catch(Exception ex)
            {
                ex.ToLog(LogLevel.Error);
            }
        }
    }
}
