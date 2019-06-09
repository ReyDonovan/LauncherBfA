namespace Ignite.Core.Components.FileSystem.Torrent
{
    public class TorrentData
    {
        public string DownloadSpeed { get; private set; }
        public string TotalRead     { get; private set; }
        public string TotalWritten  { get; private set; }
        public string DiskWriteRate { get; private set; }
        public int    Progress      { get; private set; }

        public void Build(string ds, string tr, string tw, string dwr, int progress)
        {
            DownloadSpeed = ds;
            TotalRead = tr;
            TotalWritten = tw;
            DiskWriteRate = dwr;
            Progress = progress;
        }
    }
}
