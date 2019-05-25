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
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

        public GameComponent(string server, int id)
        {
            Logger.Instance.WriteLine($"Initialize the game component with parent: {server}. In: {DateTime.Now.ToFileTimeUtc()}", LogLevel.Debug);

            ServerName = server;
            ServerId = id;

            Task.Run(() =>
            {
                System.Windows.Application.Current.Dispatcher.BeginInvoke((Action)delegate
                {
                    StatusTextDesc.Visibility = Visibility.Hidden;
                    StatusText.Text = "Подготовка ...";
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

                    Server.Text = ServerName + ": свежие новости";
                });

                var opt = SettingsMgr.Instance.GetOption(id + ".path");
                if(opt.Key != SettingsContainer.INCORRECT_OPTION)
                {
                    if (File.Exists(opt.Value + "\\Wow.exe") || File.Exists(opt.Value + "\\Wow-64.exe"))
                    {
                        System.Windows.Application.Current.Dispatcher.BeginInvoke((Action)delegate
                        {
                            StatusText.Visibility = Visibility.Hidden;
                            StatusTextDesc.Visibility = Visibility.Visible;
                            StatusTextDesc.Text = "Готов к запуску";

                            PlayButton.Content = "ИГРАТЬ";

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
                        StatusTextDesc.Text = "Необходимо указать папку с игрой";

                        PlayButton.Content = "НАСТРОИТЬ";
                        PlayButton.Click -= PlayButton_Click;
                        PlayButton.Click += PlayButton_Click_SetPath;

                        CheckButton.IsEnabled = false;
                    });
                }

            }).GetAwaiter();

            InitializeComponent();

            ApplicationEnv.Instance.ApplyStatus(ApplicationStatus.Normal);
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
            StatusText.Text = "Подготовка ...";
            StatusText.Visibility = Visibility.Visible;
            StatusTextDesc.Text = "Инициализация списка файлов";

            ProgressBar.Visibility = Visibility.Visible;

            CheckButton.IsEnabled = false;
            PlayButton.IsEnabled = false;

            Task.Run(() =>
            {
                var task = FileMgr.Instance.GetManifest(true, ServerId);
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
                    StatusText.Text = "Ошибка обновления";
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
                    StatusText.Text = "Файлы повреждены: ";
                    StatusTextDesc.Text = $"../{fn}";

                    FileMgr.Instance.OnDownloadStarted -= FileMgrDownloadStart;
                    FileMgr.Instance.OnDownloadStopped -= FileMgrDownloadStop;
                    FileMgr.Instance.OnStoppedProcesses -= FileMgrStopAll;
                    FileMgr.Instance.OnDownloadProcess -= FileMgrDownloadProcess;

                    FileMgr.Instance.OnDownloadStarted += FileMgrDownloadStart;
                    FileMgr.Instance.OnDownloadStopped += FileMgrDownloadStop;
                    FileMgr.Instance.OnStoppedProcesses += FileMgrStopAll;
                    FileMgr.Instance.OnDownloadProcess += FileMgrDownloadProcess;

                    FileMgr.Instance.StartUpdate();
                }
                else
                {
                    StatusText.Visibility = Visibility.Hidden;
                    StatusTextDesc.Visibility = Visibility.Visible;
                    StatusTextDesc.Text = "Готов к запуску";
                    StatusText.Text = "";

                    PlayButton.Content = "ИГРАТЬ";

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
                StatusText.Text = "Загрузка: ";

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
                    StatusTextDesc.Text = "Готов к запуску";

                    PlayButton.Content = "ИГРАТЬ";

                    PlayButton.IsEnabled = true;
                    CheckButton.IsEnabled = true;
                }
                else
                {
                    StatusText.Visibility = Visibility.Hidden;
                    StatusTextDesc.Visibility = Visibility.Visible;
                    StatusTextDesc.Text = "Ошибка обновления";

                    PlayButton.Content = "ИГРАТЬ";

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
                    StatusTextDesc.Text = "Готов к запуску";

                    PlayButton.Content = "ИГРАТЬ";

                    PlayButton.IsEnabled = true;
                    CheckButton.IsEnabled = true;
                }
                else
                {
                    StatusText.Text = "Ошибка загрузки файла: ";
                    StatusTextDesc.Visibility = Visibility.Visible;
                    StatusTextDesc.Text = $"{fn}";

                    PlayButton.Content = "ИГРАТЬ";

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
                StatusText.Text = "Проверка целостности: ";
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
                data = File.ReadAllLines(SettingsMgr.Instance.GetValue(ServerId + ".path") + "\\Wtf\\Config.wtf");
                File.Delete(SettingsMgr.Instance.GetValue(ServerId + ".path") + "\\Wtf\\Config.wtf");

                for(int i = 0; i < data.Length; i++)
                {
                    if(data[i].Contains("SET portal"))
                    {
                        data[i] = "SET portal \"77.82.86.211\"";
                    }
                }
            }
            catch(Exception)
            {
                data = new string[]
                {
                    "SET portal \"77.82.86.211\""
                };
            }

            using (FileStream io = new FileStream(SettingsMgr.Instance.GetValue(ServerId + ".path") + "\\Wtf\\Config.wtf", FileMode.OpenOrCreate))
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

            var prc = Process.Start(SettingsMgr.Instance.GetValue(ServerId + ".path") + "\\WoW.exe");

            PlayButton.IsEnabled = false;
            CheckButton.IsEnabled = false;
            
            StatusTextDesc.Text = "Игра запущена";

            prc.EnableRaisingEvents = true;
            prc.Exited += Prc_Exited;
        }

        private void Prc_Exited(object sender, EventArgs e)
        {
            System.Windows.Application.Current.Dispatcher.BeginInvoke((Action)delegate
            {
                PlayButton.IsEnabled = true;
                CheckButton.IsEnabled = true;

                StatusTextDesc.Text = "Готов к запуcку";
            });
        }

        private void PlayButton_Click_SetPath(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if(dialog.ShowDialog() == DialogResult.OK)
            {
                if (FileMgr.Instance.IsWowDirectory(dialog.SelectedPath))
                {
                    PlayButton.Content = "ИГРАТЬ";

                    PlayButton.IsEnabled = false;
                    CheckButton.IsEnabled = false;

                }

                SettingsMgr.Instance.WriteOption(new Option(ServerId + ".path", dialog.SelectedPath));
                SettingsMgr.Instance.Save();

                CheckButton_Click(CheckButton, null);
            }
        }
    }
}
