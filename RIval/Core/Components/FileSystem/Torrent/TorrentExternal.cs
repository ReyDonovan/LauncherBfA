using MonoTorrent.Client;
using System;
using MonoTorrent.BEncoding;
using System.IO;
using MonoTorrent.Client.Encryption;
using System.Net;
using MonoTorrent.Dht.Listeners;
using MonoTorrent.Dht;

namespace Ignite.Core.Components.FileSystem.Torrent
{
    public class TorrentExternal
    {
        public ClientEngine Engine    { get; private set; }
        public Top10Listener Listener { get; private set; }
        public TorrentManager Manager { get; private set; }
        public BEncodedDictionary FastResume { get; private set; }
        public DhtListener DhtListener { get; private set; }
        public DhtEngine Dht { get; private set; }

        public TorrentExternal(EngineSettings settings, string node)
        {
            settings.PreferEncryption = false;
            settings.AllowedEncryption = EncryptionTypes.All;

            Engine   = new ClientEngine(settings);
            Engine.ChangeListenEndpoint(new IPEndPoint(IPAddress.Any, 12999));

            byte[] nodes = null;
            try
            {
                nodes = File.ReadAllBytes(node);
            }
            catch
            {
                Console.WriteLine("No existing dht nodes could be loaded");
            }

            DhtListener = new DhtListener(new IPEndPoint(IPAddress.Any, 12999));
            Dht = new DhtEngine(DhtListener);
            Engine.RegisterDht(Dht);
            DhtListener.Start();
            Engine.DhtEngine.Start(nodes);

            Listener = new Top10Listener(10);
        }

        public void AppendManager(MonoTorrent.Common.Torrent torrent, string savepath, TorrentSettings torrentSettings)
        {
            Manager = new TorrentManager(torrent, savepath, torrentSettings);
        }

        public bool TryGetFastResume(string fastResumePath)
        {
            try
            {
                FastResume = BEncodedValue.Decode<BEncodedDictionary>(File.ReadAllBytes(fastResumePath));
            }
            catch
            {
                FastResume = new BEncodedDictionary();
            }

            return FastResume.Count > 0;
        }
    }
}
