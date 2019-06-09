using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ignite.Core.Components.FileSystem.Torrent
{
    public class TorrentDownloaderSettings
    {
        public string DownloadPath   { get; private set; }
        public string CachePath      { get; private set; }
        public string FastResumeFile { get; private set; }
        public int MaxDownloadSpeed  { get; private set; }
        public int MaxUploadSpeed    { get; private set; }
        public int ServerId          { get; private set; }

        public static TorrentDownloaderSettings Build(string download, string cache, string frFile, int serverId, int dspeed = 0, int uspeed = 0)
        {
            return new TorrentDownloaderSettings().Rebase(download, cache, frFile, serverId, dspeed, uspeed);
        }

        public TorrentDownloaderSettings Rebase(string download, string cache, string frFile, int serverId, int dspeed = 0, int uspeed = 0)
        {
            DownloadPath = download;
            CachePath = cache;
            FastResumeFile = frFile;
            MaxDownloadSpeed = dspeed;
            MaxUploadSpeed = uspeed;
            ServerId = serverId;

            return this;
        }

        public string GetResumeFile() => CachePath + "\\" + FastResumeFile;
    }
}
