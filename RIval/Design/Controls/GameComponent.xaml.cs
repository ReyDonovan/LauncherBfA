using Ignite.Core;
using Ignite.Core.Components;
using Ignite.Core.Components.News;
using Ignite.Core.Repositories;
using Ignite.Core.Settings;
using Ignite.Design.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Ignite.Core.Components.Game;

namespace Ignite.Design.Controls
{
    /// <summary>
    /// Логика взаимодействия для NewsComponent.xaml
    /// </summary>
    public partial class GameComponent : System.Windows.Controls.UserControl
    {
        public int ServerId = -1;
        private string ServerName = "%server%";
        private List<NewsRepository> News = new List<NewsRepository>();
        private GameCfg GameSettings;

        public GameComponent(string server, int id)
        {
            Logger.Instance.WriteLine($"Initialize the game component with parent: {server}. In: {DateTime.Now.ToFileTimeUtc()}", LogLevel.Debug);

            BootSettings();

            ServerName = server;
            ServerId = id;

            Task.Run(() =>
            {
                System.Windows.Application.Current.Dispatcher.BeginInvoke((Action)delegate
                {
                    StatusTextDesc.Visibility = Visibility.Hidden;

                    StatusText.Text = LanguageMgr.Instance.ValueOf("StatusText_Init");
                    PlayButton.Content = LanguageMgr.Instance.ValueOf("PlayButton");
                    CheckButton.Content = LanguageMgr.Instance.ValueOf("CheckButton");

                    PercentStatus.Visibility = Visibility.Hidden;

                    OnLoading();
                });

                News = NewsFacade.Instance.LoadNews();
                if(News.Count > 0)
                {
                    AddNews();
                }

                System.Windows.Application.Current.Dispatcher.BeginInvoke((Action)delegate
                {
                    OnReady();

                    Server.Text = string.Format(LanguageMgr.Instance.ValueOf("GameComponentTitle"), ServerName);
                });


                var opt = GameSettings.GetFolder(id);
                if(opt != null)
                {
                    if (FileMgr.Instance.IsWowDirectory(opt))
                    {
                        System.Windows.Application.Current.Dispatcher.BeginInvoke((Action)delegate
                        {
                            StatusText.Visibility = Visibility.Hidden;
                            StatusTextDesc.Visibility = Visibility.Visible;
                            StatusTextDesc.Text = LanguageMgr.Instance.ValueOf("StatusText_Ready");

                            PlayButton.Content = LanguageMgr.Instance.ValueOf("PlayButton");

                            PlayButton.IsEnabled = true;
                            CheckButton.IsEnabled = true;
                        });
                    }
                }
                else
                {
                    System.Windows.Application.Current.Dispatcher.BeginInvoke((Action)delegate
                    {
                        StatusText.Visibility = Visibility.Hidden;
                        StatusTextDesc.Visibility = Visibility.Visible;
                        StatusTextDesc.Text = LanguageMgr.Instance.ValueOf("StatusText_ChooseGame");

                        PlayButton.Content = LanguageMgr.Instance.ValueOf("ButtonChooseGame");
                        PlayButton.Click -= PlayButton_Click;
                        PlayButton.Click += PlayButton_Click_SetPath;

                        CheckButton.IsEnabled = false;
                    });
                }

            }).GetAwaiter();

            InitializeComponent();

            ApplicationEnv.Instance.ApplyStatus(ApplicationStatus.Normal);
        }

        private void BootSettings()
        {
            GameSettings = CfgMgr.Instance.GetProvider().Read<GameCfg>(false);
            if (GameSettings == null)
            {
                GameSettings = CfgMgr.Instance.GetProvider().Read<GameCfg>(true);
                if (GameSettings == null)
                {
                    CfgMgr.Instance.GetProvider().Add(new GameCfg(), new GameCfg()).Build();

                    BootSettings();
                }
            }
        }

        private void AddNews()
        {
            int startPos = 8;
            int posVal = 340;

            foreach(var element in News)
            {
                System.Windows.Application.Current.Dispatcher.BeginInvoke((Action)delegate
                {
                    NewsControl control = new NewsControl();
                    control.Title.Text = element.Title;
                    control.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    control.Link = element.Link;
                    control.Content.Text = element.Description;
                    control.PartiallyBluredDoge.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + "\\" + element.LocalImage));

                    control.Margin = new Thickness(startPos, 40, 0, 0);
                    startPos += posVal;

                    Dispatcher.BeginInvoke((MethodInvoker)(() =>
                    {
                        NewsScrollGrid.Children.Add(control);
                    }));
                });
            }
        }

        public static BitmapImage GetImageFromPath(FrameworkElement parent, string path)
        {
            return new BitmapImage { UriSource =  new Uri(path) };
        }

        private void OnLoading()
        {
            EmptyNews_Text.Visibility = Visibility.Hidden;
            Preloader.Visibility = Visibility.Visible;
            PlayButton.Visibility = Visibility.Hidden;
            CheckButton.Visibility = Visibility.Hidden;
            ProgressBar.Visibility = Visibility.Hidden;
            ProgressBar.Value = 0;
            AllPercentage.Visibility = Visibility.Hidden;
            AllPercentage.Value = 0;
        }
        private void OnReady()
        {
            if (News.Count <= 0)
            {
                EmptyNews_Text.Visibility = Visibility.Visible;
            }

            Preloader.Visibility = Visibility.Hidden;
            PlayButton.Visibility = Visibility.Visible;
            CheckButton.Visibility = Visibility.Visible;
            ProgressBar.Visibility = Visibility.Hidden;
            ProgressBar.Value = 0;
            AllPercentage.Visibility = Visibility.Hidden;
            AllPercentage.Value = 0;

            StatusText.Text = "";
        }

        private void ScrollNews_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scrollviewer = sender as ScrollViewer;
            if (e.Delta > 0)
                scrollviewer.LineLeft();
            else
                scrollviewer.LineRight();

            e.Handled = true;
        }

        private void CheckButton_Click(object sender, RoutedEventArgs e)
        {
            StatusText.Text = LanguageMgr.Instance.ValueOf("StatusText_Init");
            StatusText.Visibility = Visibility.Visible;
            StatusTextDesc.Text = LanguageMgr.Instance.ValueOf("DescText_Init");

            ProgressBar.Visibility = Visibility.Visible;

            CheckButton.IsEnabled = false;
            PlayButton.IsEnabled = false;

            Task.Run(() =>
            {
                var task = FileMgr.Instance.GetManifest(true, ServerId, GameSettings.GetFolder(ServerId));
                task.GetAwaiter();

                if (task.GetAwaiter().GetResult())
                {
                    FileMgr.Instance.OnCheckStarted -= FileMgrCheckStart;
                    FileMgr.Instance.OnCheckStopped -= FileMgrCheckStop;

                    FileMgr.Instance.OnCheckStarted += FileMgrCheckStart;
                    FileMgr.Instance.OnCheckStopped += FileMgrCheckStop;

                    FileMgr.Instance.StartCheck();
                }
                else
                {
                    StatusText.Text = LanguageMgr.Instance.ValueOf("StatusText_UpdateError");
                    StatusText.Visibility = Visibility.Visible;
                    StatusTextDesc.Text = "";
                }
            }).Wait();
        }

        private void FileMgrCheckStop(string fn, bool result)
        {
            System.Windows.Application.Current.Dispatcher.BeginInvoke((Action)delegate
            {
                if (!result)
                {
                    StatusText.Text = LanguageMgr.Instance.ValueOf("StatusText_FilesDamaged");
                    StatusTextDesc.Text = $"../{fn}";

                    FileMgr.Instance.OnDownloadStarted -= FileMgrDownloadStart;
                    FileMgr.Instance.OnDownloadStopped -= FileMgrDownloadStop;
                    FileMgr.Instance.OnStoppedProcesses -= FileMgrStopAll;
                    FileMgr.Instance.OnDownloadProcess -= FileMgrDownloadProcess;

                    FileMgr.Instance.OnDownloadStarted += FileMgrDownloadStart;
                    FileMgr.Instance.OnDownloadStopped += FileMgrDownloadStop;
                    FileMgr.Instance.OnStoppedProcesses += FileMgrStopAll;
                    FileMgr.Instance.OnDownloadProcess += FileMgrDownloadProcess;

                    FileMgr.Instance.StartUpdate(GameSettings.GetFolder(ServerId));
                }
                else
                {
                    StatusText.Visibility = Visibility.Hidden;
                    StatusTextDesc.Visibility = Visibility.Visible;
                    StatusTextDesc.Text = LanguageMgr.Instance.ValueOf("StatusText_Ready");
                    StatusText.Text = "";

                    PlayButton.Content = LanguageMgr.Instance.ValueOf("PlayButton");

                    PlayButton.IsEnabled = true;
                    CheckButton.IsEnabled = true;
                }

                ProgressBar.Visibility = Visibility.Hidden;
                AllPercentage.Visibility = Visibility.Hidden;
                AllPercentage.Value = 0;
            });
        }

        private void FileMgrDownloadProcess(string info, int percentage, int currentFilePercantage)
        {
            System.Windows.Application.Current.Dispatcher.BeginInvoke((Action)delegate
            {
                StatusText.Visibility = Visibility.Visible;
                StatusText.Text = LanguageMgr.Instance.ValueOf("StatusText_Download");

                StatusTextDesc.Text = info;
                PercentStatus.Text = $"{percentage}%";

                PercentStatus.Visibility = Visibility.Visible;
                ProgressBar.Visibility = Visibility.Visible;
                ProgressBar.Value = percentage;
                AllPercentage.Visibility = Visibility.Visible;
                AllPercentage.Value = currentFilePercantage;

                CheckButton.IsEnabled = false;
                PlayButton.IsEnabled = false;

                WindowMgr.Instance.Run<MainWindow>((window) =>
                {
                    window.SwitchMenuButtons(false);
                });
            });
        }

        private void FileMgrStopAll(bool result)
        {
            System.Windows.Application.Current.Dispatcher.BeginInvoke((Action)delegate
            {
                ProgressBar.Value = 0;
                ProgressBar.Visibility = Visibility.Hidden;
                PercentStatus.Visibility = Visibility.Hidden;
                AllPercentage.Visibility = Visibility.Hidden;
                AllPercentage.Value = 0;

                if (result)
                {
                    StatusText.Visibility = Visibility.Hidden;
                    StatusTextDesc.Visibility = Visibility.Visible;
                    StatusTextDesc.Text = LanguageMgr.Instance.ValueOf("StatusText_Ready");

                    PlayButton.Content = LanguageMgr.Instance.ValueOf("PlayButton");

                    PlayButton.IsEnabled = true;
                    CheckButton.IsEnabled = true;
                }
                else
                {
                    StatusText.Visibility = Visibility.Hidden;
                    StatusTextDesc.Visibility = Visibility.Visible;
                    StatusTextDesc.Text = LanguageMgr.Instance.ValueOf("StatusText_UpdateError");

                    PlayButton.Content = LanguageMgr.Instance.ValueOf("PlayButton");

                    PlayButton.IsEnabled = false;
                    CheckButton.IsEnabled = true;
                }
            });
        }

        private void FileMgrDownloadStop(string fn, bool result)
        {
            System.Windows.Application.Current.Dispatcher.BeginInvoke((Action)delegate
            {
                ProgressBar.Value = 0;
                ProgressBar.Visibility = Visibility.Hidden;
                PercentStatus.Visibility = Visibility.Hidden;
                AllPercentage.Visibility = Visibility.Hidden;
                AllPercentage.Value = 0;

                if (result)
                {
                    StatusText.Visibility = Visibility.Hidden;
                    StatusTextDesc.Visibility = Visibility.Visible;
                    StatusTextDesc.Text = LanguageMgr.Instance.ValueOf("StatusText_Ready");

                    PlayButton.Content = LanguageMgr.Instance.ValueOf("PlayButton");

                    PlayButton.IsEnabled = true;
                    CheckButton.IsEnabled = true;
                }
                else
                {
                    StatusText.Text = LanguageMgr.Instance.ValueOf("StatusText_FilesDamaged_One");
                    StatusTextDesc.Visibility = Visibility.Visible;
                    StatusTextDesc.Text = $"{fn}";

                    PlayButton.Content = LanguageMgr.Instance.ValueOf("PlayButton");

                    PlayButton.IsEnabled = false;
                    CheckButton.IsEnabled = true;
                }
            });
        }

        private void FileMgrDownloadStart(string fn)
        {
            System.Windows.Application.Current.Dispatcher.BeginInvoke((Action)delegate
            {
                StatusText.Text = "Загрузка: ";
                StatusTextDesc.Text = $"{fn}";
            });
        }

        private void FileMgrCheckStart(string fn, int percent)
        {
            System.Windows.Application.Current.Dispatcher.BeginInvoke((Action)delegate
            {
                StatusText.Text = LanguageMgr.Instance.ValueOf("StatusText_CheckFileBuilds");
                StatusTextDesc.Text = $"../{fn}";

                ProgressBar.Visibility = Visibility.Visible;
                ProgressBar.Value = percent;
            });
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            string[] data = null;

            try
            {
                data = File.ReadAllLines(GameSettings.GetFolder(ServerId) + "\\Wtf\\Config.wtf");
                File.Delete(GameSettings.GetFolder(ServerId) + "\\Wtf\\Config.wtf");

                for(int i = 0; i < data.Length; i++)
                {
                    if(data[i].Contains("SET portal"))
                    {
                        data[i] = $"SET portal \"{ApplicationEnv.Instance.GetPortal(ServerId)}\"";
                    }
                }
            }
            catch(Exception)
            {
                data = new string[]
                {
                    $"SET portal \"{ApplicationEnv.Instance.GetPortal(ServerId)}\""
                };
            }

            using (FileStream io = new FileStream(GameSettings.GetFolder(ServerId) + "\\Wtf\\Config.wtf", FileMode.OpenOrCreate))
            {
                bool adjusted = false;

                foreach (var line in data)
                {
                    if (line.Contains("SET portal") && adjusted) continue;
                    if (line.Contains("SET portal") && !adjusted)
                    {
                        adjusted = true;
                    }

                    byte[] buffer = Encoding.UTF8.GetBytes(line + Environment.NewLine);
                    io.Write(buffer, 0, buffer.Length);
                }
            }

            var prc = Process.Start(GameSettings.GetFolder(ServerId) + "\\WoW.exe");

            PlayButton.IsEnabled = false;
            CheckButton.IsEnabled = false;
            
            StatusTextDesc.Text = LanguageMgr.Instance.ValueOf("StatusText_GameStarted");

            prc.EnableRaisingEvents = true;
            prc.Exited += Prc_Exited;
        }

        private void Prc_Exited(object sender, EventArgs e)
        {
            System.Windows.Application.Current.Dispatcher.BeginInvoke((Action)delegate
            {
                PlayButton.IsEnabled = true;
                CheckButton.IsEnabled = true;

                StatusTextDesc.Text = LanguageMgr.Instance.ValueOf("StatusText_Ready");
            });
        }

        private void PlayButton_Click_SetPath(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if(dialog.ShowDialog() == DialogResult.OK)
            {
                if (FileMgr.Instance.IsWowDirectory(dialog.SelectedPath))
                {
                    PlayButton.Content = LanguageMgr.Instance.ValueOf("PlayButton");

                    PlayButton.IsEnabled = false;
                    CheckButton.IsEnabled = false;

                }

                GameSettings.AddFolder(ServerId, dialog.SelectedPath);
                CfgMgr.Instance.GetProvider().Write(GameSettings, false);

                CheckButton_Click(CheckButton, null);
            }
        }
    }
}
