using IX.Spider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ignite.Core
{
    interface INetworkAccessor
    {
        void Connect(string host, int port);
        void Disconnect();
        void SendRaw(SpiderNetwork packet);
        Network.NetworkMgr SendOnMgr(SpiderNetwork packet);
    }
}
