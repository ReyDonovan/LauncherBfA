using IX.Composer.Architecture;
using Ignite.Core.Components;
using Ignite.Design.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

namespace Ignite.Core
{
    public class WindowMgr : Singleton<WindowMgr>
    {
        public static List<IWindowDispatcher> HostedWindows  { get; private set; } = new List<IWindowDispatcher>();
        public static List<GameComponent>     HostedControls { get; private set; } = new List<GameComponent>();

        public void AddHosted(IWindowDispatcher window)
        {
            HostedWindows.Add(window);

            Logger.Instance.WriteLine($"Added hosted: {window.GetType().FullName}", LogLevel.Info);
        }
        public IWindowDispatcher GetHosted<T>() where T : Window
        {
            IWindowDispatcher obj = null;
            for (int i = 0; i < HostedWindows.Count; i++)
            {
                if (typeof(T) == HostedWindows[i].GetType())
                {
                    obj = HostedWindows[i];

                    break;
                }
            }

            return obj;
        }

        public void AppendLanguages()
        {
            HostedWindows.ForEach((element) => element.AppendLocale(LanguageMgr.Instance));
        }

        public GameComponent GetCachedGameComponent(int id)
        {
            GameComponent obj = null;
            try
            {
                for (int i = 0; i < HostedControls.Count; i++)
                {
                    if (id == HostedControls[i].ServerId)
                    {
                        obj = HostedControls[i];

                        break;
                    }
                }
            }
            catch(Exception) { }

            return obj;
        }
        public void RemoveCachedGameComponent(int id)
        {
            HostedControls.Clear();
        }
        public void CacheGameComponent(GameComponent comp, int id)
        {
            if(!HostedControls.Any((element) => element.ServerId == id))
            {
                HostedControls.Add(comp);
            }
        }

        public void Dispatch(ApplicationStatus status)
        {
            switch (status)
            {
                case ApplicationStatus.Banned:
                    {
                        HostedWindows.ForEach((element) => element.Banned());

                        break;
                    }

                case ApplicationStatus.Blocked:
                    {
                        HostedWindows.ForEach((element) => element.Blocked());

                        break;
                    }

                case ApplicationStatus.Connected:
                    {
                        HostedWindows.ForEach((element) => element.Connected());

                        break;
                    }

                case ApplicationStatus.Disconnected:
                    {
                        HostedWindows.ForEach((element) => element.Disconnect());

                        break;
                    }

                case ApplicationStatus.Loading:
                    {
                        HostedWindows.ForEach((element) => element.Loading());

                        break;
                    }

                case ApplicationStatus.Normal:
                    {
                        HostedWindows.ForEach((element) => element.Normal());

                        break;
                    }

                default:
                    {
                        ArgumentException ex = new ArgumentException($"Incorrect appstatus '{status.ToString()}' for changing.");
                        ex.ToLog(Components.LogLevel.Error);

                        throw ex;
                    }
            }
        }
        public void Run<T>(Action<T> action) where T : Window
        {
            Window obj = null;
            for (int i = 0; i < HostedWindows.Count; i++)
            {
                if (typeof(T) == HostedWindows[i].GetType())
                {
                    obj = HostedWindows[i].ToWindow();

                    break;
                }
            }

            if (obj != null)
            {
                try
                {
                    obj.Dispatcher.BeginInvoke((MethodInvoker)(() =>
                    {
                        action((T)obj);
                    }));

                }
                catch (Exception internalEx)
                {
                    internalEx.ToLog(LogLevel.Error);
                }

                return;
            }

            Exception ex = new Exception($"Window '{nameof(T)}' not founded in hosted windows.");
            ex.ToLog(LogLevel.Error);

            throw ex;
        }
    }
}
