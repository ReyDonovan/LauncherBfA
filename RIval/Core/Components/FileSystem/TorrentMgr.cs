using Ignite.Core.Components.FileSystem.Torrent;
using System.IO;
using System.Threading.Tasks;

namespace Ignite.Core.Components.FileSystem
{
    public class TorrentMgr : Singleton<TorrentMgr>
    {
        private TorrentDownloader Downloader { get; set; }

        public TorrentMgr()
        {
            Downloader = new TorrentDownloader();

            if (!Directory.Exists("cache\\fs_tr"))
                 Directory.CreateDirectory("cache\\fs_tr");
        }

        public bool Download(string gamePath, int serverId)
        {
            Downloader.Boot(TorrentDownloaderSettings.Build(
                gamePath, 
                "cache\\fs_tr", 
                "cd436cc6804df0ef6bc6d138435b8331f83f0934d1a96f6900f660f54680bcea.data", 
                serverId,
                0, 
                0));

            return Downloader.Download();
        }

        public async Task<bool> DownloadAsync(string gamePath, int serverId)
        {
            Downloader.Boot(TorrentDownloaderSettings.Build(
                gamePath, 
                "cache\\fs_tr", 
                "cd436cc6804df0ef6bc6d138435b8331f83f0934d1a96f6900f660f54680bcea.data", 
                serverId,
                0, 
                0));

            return await Downloader.DownloadAsync();
        }

        public void Subscribe<T>(T handler)
        {
            if (typeof(T) == typeof(OnPeerAdded))
            {
                Downloader.SubscribePeer((OnPeerAdded)(object)handler);
            }
            else if (typeof(T) == typeof(OnTorrentChangeState))
            {
                Downloader.SubscribeState((OnTorrentChangeState)(object)handler);
            }
            else if(typeof(T) == typeof(OnDownloadStopped))
            {
                Downloader.SubscribeDownload((OnDownloadStopped)(object)handler);
            }
            else
            { }
        }
        public void Unsubscribe<T>(T handler)
        {
            if (typeof(T) == typeof(OnPeerAdded))
            {
                Downloader.UnsubscribePeer((OnPeerAdded)(object)handler);
            }
            else if (typeof(T) == typeof(OnTorrentChangeState))
            {
                Downloader.UnsubscribeState((OnTorrentChangeState)(object)handler);
            }
            else if (typeof(T) == typeof(OnDownloadStopped))
            {
                Downloader.UnsubscribeDownload((OnDownloadStopped)(object)handler);
            }
            else
            { }
        }
    }
}
