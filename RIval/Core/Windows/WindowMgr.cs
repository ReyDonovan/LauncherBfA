using IX.Composer.Architecture;
using RIval.Core.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace RIval.Core
{
    public class WindowMgr : Singleton<WindowMgr>
    {
        public static List<IWindowDispatcher> HostedWindows { get; private set; } = new List<IWindowDispatcher>();

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
                    internalEx.ToLog(Components.LogLevel.Error);
                }

                return;
            }

            Exception ex = new Exception($"Window '{typeof(T).FullName}' not founded in hosted windows.");
            ex.ToLog(Components.LogLevel.Error);

            throw ex;
        }
    }
}
