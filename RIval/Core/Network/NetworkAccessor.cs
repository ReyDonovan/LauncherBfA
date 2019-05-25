using IX.Spider;
using Ignite.Core.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Ignite.Core
{
    public enum DisconnectedReasons
    {
        TimeOut,
        Banned,
        Server,
        Normal,
        Unnamed
    }
    public class BaseNetworkAccessor : Singleton<BaseNetworkAccessor>, INetworkAccessor
    {
        #region Constants
        /// <summary>
        /// Определеяет хост подключения по умолчанию для метода <see cref="Connect(string, int)"/>
        /// </summary>
        public const string HOST = "127.0.0.1";

        /// <summary>
        /// Определеяет порт подключения по умолчанию для метода <see cref="Connect(string, int)"/>
        /// </summary>
        public const int PORT = 8789;
        #endregion

        #region Delegates & Events
        public delegate void ConnectionDelegate();
        public delegate void DisconnectionDelegate(DisconnectedReasons reason);

        private event ConnectionDelegate OnConnected;
        private event DisconnectionDelegate OnDisconnected;
        #endregion

        #region Public
        public NetworkMgr Manager { get; private set; }
        #endregion

        #region Private
        private NetScheduler Service { get; set; }
        #endregion

        /// <summary>
        /// Выполняет подключение к текущему серверу
        /// </summary>
        /// <param name="host">Хост для подключения</param>
        /// <param name="port"></param>
        public void Connect(string host, int port)
        {
            try
            {
                if(Service == null)
                {
                    Service = new NetScheduler();
                }

                Service.Connect(host, port);
                Service.OnConnect += Service_OnConnect;
                Service.OnDataReceived += Service_OnDataReceived;
                Service.OnDisconnect += Service_OnDisconnect;

                OnConnected?.Invoke();
            }   
            catch(SocketException)
            {
                OnDisconnected?.Invoke(DisconnectedReasons.TimeOut);
            }
            catch(Exception ex)
            {
                Components.Logger.Instance.WriteLine(ex.ToString(), Components.LogLevel.Error);
                OnDisconnected?.Invoke(DisconnectedReasons.Unnamed);
            }
        }

        private void Service_OnDisconnect(object sender, NetScheduler connection)
        {
            throw new NotImplementedException();
        }

        private void Service_OnDataReceived(object sender, NetScheduler connection, SpiderNetwork e, string key)
        {
            throw new NotImplementedException();
        }

        private void Service_OnConnect(object sender, NetScheduler connection)
        {
            throw new NotImplementedException();
        }

        public void Disconnect()
        {
            try
            {
                Service.Disconnect();

                OnDisconnected?.Invoke(DisconnectedReasons.Normal);
            }
            catch(SocketException)
            {
                OnDisconnected?.Invoke(DisconnectedReasons.Unnamed);
            }
            catch(Exception ex)
            {
                Components.Logger.Instance.WriteLine(ex.ToString(), Components.LogLevel.Error);
                OnDisconnected?.Invoke(DisconnectedReasons.Unnamed);
            }
        }

        public void Subscribe(ConnectionDelegate @delegate)
        {
            if(!OnConnected.GetInvocationList().Contains(@delegate))
            {
                OnConnected += @delegate;
            }
        }
        public void Subscribe(DisconnectionDelegate @delegate)
        {
            if(!OnDisconnected.GetInvocationList().Contains(@delegate))
            {
                OnDisconnected += @delegate;
            }
        }
        public void Unsubscribe(ConnectionDelegate @delegate)
        {
            if (OnConnected.GetInvocationList().Contains(@delegate))
            {
                OnConnected -= @delegate;
            }
        }
        public void Unsubscribe(DisconnectionDelegate @delegate)
        {
            if (OnDisconnected.GetInvocationList().Contains(@delegate))
            {
                OnDisconnected -= @delegate;
            }
        }

        public void SendRaw(SpiderNetwork packet)
        {
            throw new NotImplementedException();
        }
        public NetworkMgr SendOnMgr(SpiderNetwork packet)
        {
            throw new NotImplementedException();
        }
    }
}
