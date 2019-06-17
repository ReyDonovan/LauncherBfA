using Ignite.Core;
using Ignite.Core.Components;
using Ignite.Core.Components.News;
using Ignite.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Ignite.Core.Components.FileSystem.Torrent;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Ignite.Core.Components.Game;
using Ignite.Core.Components.FileSystem.Additions;
using Ignite.Core.Components.Message;
using Ignite.Core.Components.Launcher;
using Ignite.Core.Components.FileSystem;

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
            InitializeComponent();

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
                    else
                    {
                        System.Windows.Application.Current.Dispatcher.BeginInvoke((Action)delegate
                        {
                            StatusText.Visibility = Visibility.Hidden;
                            StatusTextDesc.Visibility = Visibility.Visible;
                            StatusTextDesc.Text = LanguageMgr.Instance.ValueOf("StatusText_FilesDamaged");

                            PlayButton.Content = LanguageMgr.Instance.ValueOf("PlayButton");

                            PlayButton.IsEnabled = false;
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

        private async void CheckButton_Click(object sender, RoutedEventArgs e)
        {
            WindowMgr.Instance.Run<MainWindow>((window) =>
            {
                window.SwitchMenuButtons(false);
            });

            StatusText.Text = LanguageMgr.Instance.ValueOf("StatusText_Init");
            StatusText.Visibility = Visibility.Visible;
            StatusTextDesc.Visibility = Visibility.Visible;
            StatusTextDesc.Text = LanguageMgr.Instance.ValueOf("DescText_Init");

            ProgressBar.Visibility = Visibility.Visible;

            CheckButton.IsEnabled = false;
            PlayButton.IsEnabled = false;

            if (FileMgr.Instance.IsEnoughFreeSpace(GameSettings.GetFolder(ServerId)))
            {
                if (await FileMgr.Instance.GetManifest(true, ServerId, GameSettings.GetFolder(ServerId)))
                {
                    FileMgr.Instance.Subscribe<FileChecker.FileCheckerProcess>(DoFileCheck);

                    if (await FileMgr.Instance.CheckAsync())
                    {
                        StatusText.Visibility = Visibility.Hidden;
                        StatusTextDesc.Visibility = Visibility.Visible;

                        PercentStatus.Text = $"0%";
                        PercentStatus.Visibility = Visibility.Hidden;

                        StatusTextDesc.Text = LanguageMgr.Instance.ValueOf("StatusText_Ready");

                        PlayButton.Content = LanguageMgr.Instance.ValueOf("PlayButton");

                        PlayButton.IsEnabled = true;
                        CheckButton.IsEnabled = true;
                    }
                    else
                    {
                        StatusText.Visibility = Visibility.Hidden;
                        StatusTextDesc.Visibility = Visibility.Visible;
                        StatusTextDesc.Text = LanguageMgr.Instance.ValueOf("StatusText_FilesDamaged");

                        PlayButton.Content = LanguageMgr.Instance.ValueOf("PlayButton");

                        PlayButton.IsEnabled = false;
                        CheckButton.IsEnabled = false;

                        MessageBoxMgr.Instance.ShowWarning(
                            LanguageMgr.Instance.ValueOf("MainWindow_Downloading_MBStartHeader"),
                            LanguageMgr.Instance.ValueOf("MainWindow_Downloading_MBStartDesc"));

                        StartUpdate();
                    }

                    FileMgr.Instance.Unsubscribe<FileChecker.FileCheckerProcess>(DoFileCheck);
                }
                else
                {
                    StatusTextDesc.Text = LanguageMgr.Instance.ValueOf("MainWindow_DownloadStop_Error_Title");

                    CheckButton.IsEnabled = true;
                    PlayButton.IsEnabled = false;

                    MessageBoxMgr.Instance.ShowCriticalError(LanguageMgr.Instance.ValueOf("MainWindow_DownloadStop_Error_Title"), LanguageMgr.Instance.ValueOf("MainWindow_DownloadStop_Error_Desc"));
                }
            }
            else
            {
                StatusTextDesc.Text = LanguageMgr.Instance.ValueOf("MainWindow_EnoughSpace_Title");

                CheckButton.IsEnabled = true;
                PlayButton.IsEnabled = false;

                MessageBoxMgr.Instance.ShowCriticalError(LanguageMgr.Instance.ValueOf("MainWindow_EnoughSpace_Title"), LanguageMgr.Instance.ValueOf("MainWindow_EnoughSpace_Desc"));
            }

            ProgressBar.Visibility = Visibility.Hidden;
            StatusText.Visibility = Visibility.Hidden;
            StatusTextDesc.Visibility = Visibility.Visible;

            WindowMgr.Instance.Run<MainWindow>((window) =>
            {
                window.SwitchMenuButtons(true);
            });
        }

        [Obsolete]
        private async void StartUpdate1()
        {
            ProgressBar.Visibility = Visibility.Hidden;
            StatusText.Visibility = Visibility.Visible;
            StatusTextDesc.Visibility = Visibility.Hidden;
            CheckButton.IsEnabled = false;
            PlayButton.IsEnabled = false;

            //TODO: Start update
            FileMgr.Instance.Subscribe<FileDownloader.FileDownloaderProcess>(DoFileDownload);

            if(await FileMgr.Instance.DownloadAsync())
            {
                StatusText.Visibility = Visibility.Hidden;
                StatusTextDesc.Visibility = Visibility.Visible;
                StatusTextDesc.Text = LanguageMgr.Instance.ValueOf("StatusText_Ready");

                PlayButton.Content = LanguageMgr.Instance.ValueOf("PlayButton");

                PlayButton.IsEnabled = true;
                CheckButton.IsEnabled = true;

                MessageBoxMgr.Instance.ShowSuccess(LanguageMgr.Instance.ValueOf("MainWindow_DownloadStop_Success_Title"), LanguageMgr.Instance.ValueOf("MainWindow_DownloadStop_Success_Desc"));
            }
            else
            {
                StatusText.Text = LanguageMgr.Instance.ValueOf("StatusText_FilesDamaged_One");
                StatusTextDesc.Visibility = Visibility.Visible;

                PlayButton.Content = LanguageMgr.Instance.ValueOf("PlayButton");

                PlayButton.IsEnabled = false;
                CheckButton.IsEnabled = true;

                MessageBoxMgr.Instance.ShowCriticalError(LanguageMgr.Instance.ValueOf("MainWindow_DownloadStop_Error_Title"), LanguageMgr.Instance.ValueOf("MainWindow_DownloadStop_Error_Desc"));
            }

            ProgressBar.Visibility = Visibility.Hidden;
            ProgressBar.Value = 0;
            PercentStatus.Text = $"0%";
            PercentStatus.Visibility = Visibility.Hidden;
        }

        private async void StartUpdate()
        {
            ProgressBar.Visibility = Visibility.Hidden;
            StatusText.Visibility = Visibility.Visible;
            StatusTextDesc.Visibility = Visibility.Hidden;
            CheckButton.IsEnabled = false;
            PlayButton.IsEnabled = false;

            TorrentMgr.Instance.Subscribe<OnTorrentChangeState>(DoTorrentDownload);
            TorrentMgr.Instance.Subscribe<OnDownloadStopped>(DoDownloadStopped);

            if (await TorrentMgr.Instance.DownloadAsync(GameSettings.GetFolder(ServerId), ServerId))
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
                StatusTextDesc.Text = LanguageMgr.Instance.ValueOf("StatusText_FilesDamaged");
                StatusTextDesc.Visibility = Visibility.Visible;

                PlayButton.Content = LanguageMgr.Instance.ValueOf("PlayButton");

                PlayButton.IsEnabled = false;
                CheckButton.IsEnabled = true;

                PercentStatus.Text = $"0%";
                PercentStatus.Visibility = Visibility.Hidden;

                MessageBoxMgr.Instance.ShowCriticalError(LanguageMgr.Instance.ValueOf("MainWindow_DownloadStop_Error_Title"), LanguageMgr.Instance.ValueOf("MainWindow_DownloadStop_Error_Desc"));
            }

            ProgressBar.Visibility = Visibility.Hidden;
            ProgressBar.Value = 0;
            PercentStatus.Text = $"0%";
            PercentStatus.Visibility = Visibility.Hidden;

            TorrentMgr.Instance.Unsubscribe<OnTorrentChangeState>(DoTorrentDownload);
        }


        private void DoFileCheck(string filename, int percentage)
        {
            System.Windows.Application.Current.Dispatcher.BeginInvoke((Action)delegate
            {
                StatusText.Text = LanguageMgr.Instance.ValueOf("StatusText_CheckFileBuilds");
                StatusTextDesc.Text = $"../{filename}";

                PercentStatus.Visibility = Visibility.Visible;
                PercentStatus.Text = $"{percentage}%";
                ProgressBar.Visibility = Visibility.Visible;
                ProgressBar.Value = percentage;
            });
        }
        private void DoDownloadStopped()
        {
            TorrentMgr.Instance.Subscribe<OnDownloadStopped>(DoDownloadStopped);

            System.Windows.Application.Current.Dispatcher.BeginInvoke((Action)delegate
            {
                StatusText.Visibility = Visibility.Hidden;
                StatusTextDesc.Visibility = Visibility.Visible;
                StatusTextDesc.Text = LanguageMgr.Instance.ValueOf("StatusText_Ready");

                PlayButton.Content = LanguageMgr.Instance.ValueOf("PlayButton");

                PlayButton.IsEnabled = true;
                CheckButton.IsEnabled = true;

                PercentStatus.Visibility = Visibility.Hidden;
                ProgressBar.Visibility = Visibility.Hidden;
                ProgressBar.Value = 0;

                MessageBoxMgr.Instance.ShowSuccess(LanguageMgr.Instance.ValueOf("MainWindow_DownloadStop_Success_Title"), LanguageMgr.Instance.ValueOf("MainWindow_DownloadStop_Success_Desc"));
            });
        }
        private void DoTorrentDownload(TorrentData info)
        {
            System.Windows.Application.Current.Dispatcher.BeginInvoke((Action)delegate
            {
                PlayButton.IsEnabled = false;
                CheckButton.IsEnabled = false;

                StatusText.Visibility = Visibility.Visible;
                StatusText.Text = LanguageMgr.Instance.ValueOf("StatusText_Download");

                if (info.DownloadSpeed.Split(' ')[0] != "0")
                {
                    StatusTextDesc.Text = $"{string.Format(LanguageMgr.Instance.ValueOf("Download_Speed_Title"), info.DownloadSpeed)}";
                    StatusTextDesc.Text += $" {string.Format(LanguageMgr.Instance.ValueOf("Download_Speed_Downloaded"), info.TotalRead)}";
                    PercentStatus.Text = $"{info.Progress}%";
                }
                else
                {
                    StatusTextDesc.Text = LanguageMgr.Instance.ValueOf("StatusText_GamePrepare");
                    PercentStatus.Text = $"{info.Progress}%";
                }

                PercentStatus.Visibility = Visibility.Visible;
                ProgressBar.Visibility = Visibility.Visible;
                ProgressBar.Value = info.Progress;

                WindowMgr.Instance.Run<MainWindow>((window) =>
                {
                    window.SwitchMenuButtons(false);
                });
            });
        }
        private void DoFileDownload(string info, int percentage)
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

                WindowMgr.Instance.Run<MainWindow>((window) =>
                {
                    window.SwitchMenuButtons(false);
                });
            });
        }

        private async void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            string[] data = null;

            if (FileMgr.Instance.IsWowDirectory(GameSettings.GetFolder(ServerId)))
            {

                try
                {
                    data = File.ReadAllLines(GameSettings.GetFolder(ServerId) + "\\Wtf\\Config.wtf");
                    File.Delete(GameSettings.GetFolder(ServerId) + "\\Wtf\\Config.wtf");

                    for (int i = 0; i < data.Length; i++)
                    {
                        if (data[i].Contains("SET portal"))
                        {
                            data[i] = $"SET portal \"{ApplicationEnv.Instance.GetPortal(ServerId)}\"";
                        }
                    }
                }
                catch (Exception)
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

                PlayButton.IsEnabled = false;
                CheckButton.IsEnabled = false;

                StatusTextDesc.Text = LanguageMgr.Instance.ValueOf("StatusText_GamePrepare");

                try
                {
                    if (!await LaunchMgr.Instance.Launch(GameSettings.GetFolder(ServerId)))
                    {
                        var prc = Process.Start(GameSettings.GetFolder(ServerId) + "\\Wow.exe");

                        StatusTextDesc.Text = LanguageMgr.Instance.ValueOf("StatusText_GameStarted");

                        prc.EnableRaisingEvents = true;
                        prc.Exited += Prc_Exited;
                    }
                    else
                    {
                        var prcList = Process.GetProcessesByName("Wow");
                        if(prcList.Length > 0)
                        {
                            var prc = prcList[0];

                            StatusTextDesc.Text = LanguageMgr.Instance.ValueOf("StatusText_GameStarted");

                            prc.EnableRaisingEvents = true;
                            prc.Exited += Prc_Exited;
                        }
                    }
                }
                catch (Exception ex)
                {
                    ex.ToLog(LogLevel.Error);

                    StatusTextDesc.Text = LanguageMgr.Instance.ValueOf("StatusText_GameStarted_Error");

                    PlayButton.IsEnabled = true;
                    CheckButton.IsEnabled = true;
                }
            }
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
                if(FileMgr.Instance.IsEnoughFreeSpace(dialog.SelectedPath))
                {
                    GameSettings.AddFolder(ServerId, dialog.SelectedPath);
                    CfgMgr.Instance.GetProvider().Write(GameSettings, false);

                    CheckButton_Click(CheckButton, null);
                }
                else
                {
                    MessageBoxMgr.Instance.ShowCriticalError(LanguageMgr.Instance.ValueOf("MainWindow_EnoughSpace_Title"), LanguageMgr.Instance.ValueOf("MainWindow_EnoughSpace_Desc"));
                }
            }
        }
    }
}
