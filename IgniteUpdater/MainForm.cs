using IX.Composer.Architecture;
using SharpCompress.Archives.Rar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IgniteUpdater
{
    public partial class MainForm : Form
    {
        private WebClient WebObj = new WebClient();
        private ApiFacade Api = new ApiFacade();
        private string RemoteVersion = "";

        public MainForm()
        {
            InitializeComponent();

            progressBar1.Visible = false;
            AppendDescString("");
            AppendDownloadString("");
        }

        public void Process()
        {
            progressBar1.Visible = true;
            progressBar1.Style = ProgressBarStyle.Marquee;

            AppendDescString("проверка обновлений");
            AppendDownloadString("");

            string response = Api.Builder<string>().CreateRequest(Api.GetUri("api-update-check")).GetResponse().First();

            if(IX.Composer.Architecture.Version.TryParse(response, out var remote))
            {
                if(IX.Composer.Architecture.Version.TryParse(SettingsViewer.Read("version"), out var local))
                {
                    if(local.ToString() == remote.ToString())
                    {
                        System.Diagnostics.Process.Start("Ignite.exe");
                        Environment.Exit(0);
                    }
                    else if(local < remote)
                    {
                        if(File.Exists("Ignite.exe"))
                        {
                            File.Delete("Ignite.exe");
                        }

                        WebObj.DownloadFileCompleted += WebObj_DownloadFileCompleted;
                        WebObj.DownloadProgressChanged += WebObj_DownloadProgressChanged;

                        var update = Api.Builder<UpdateInfo>().CreateRequest(Api.BuildUri("api-update-get", response)).GetResponse().First();

                        RemoteVersion = update.Version;
                        WebObj.DownloadFileAsync(new Uri(update.Link), "data.rar");
                    }
                }
            }
        }

        private void WebObj_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar1.Style = ProgressBarStyle.Blocks;
            progressBar1.Value = e.ProgressPercentage;

            AppendDescString("загрузка файла обновления");
            AppendDownloadString(e.ProgressPercentage + " %");
        }

        private void WebObj_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if(e.Error == null)
            {
                SettingsViewer.Write("version", RemoteVersion);
                progressBar1.Style = ProgressBarStyle.Marquee;

                AppendDescString("распаковка файлов");
                AppendDownloadString("");

                using (var reader = RarArchive.Open("data.rar"))
                {
                    var extractor = reader.ExtractAllEntries();
                    while(extractor.MoveToNextEntry())
                    {
                        string fileName = Path.GetFileName(extractor.Entry.Key);
                        string rootToFile = Path.GetFullPath(extractor.Entry.Key).Replace(fileName, "");

                        if (!Directory.Exists(rootToFile))
                        {
                            Directory.CreateDirectory(rootToFile);
                        }

                        try
                        {
                            using (var file = File.Create(Path.GetFullPath(extractor.Entry.Key)))
                                extractor.WriteEntryTo(file);
                        }
                        catch(Exception)
                        {
                            continue;
                        }
                    }
                }

                if(File.Exists("data.rar"))
                {
                    File.Delete("data.rar");
                }

                if(File.Exists("settings/config.stg"))
                {
                    File.Decrypt("settings/config.stg");
                }

                AppendDescString("готово к запуску");

                System.Diagnostics.Process.Start("Ignite.exe");
                Environment.Exit(0);
            }
            else
            {
                MessageBox.Show(e.Error.ToString());
            }
        }

        public void AppendDescString(string text)
        {
            downloadDescString.Text = $"-- {text}";
        }

        public void AppendDownloadString(string readyText)
        {
            downloadInfoString.Text = readyText;
        }

        private void MainForm_Shown(object sender, System.EventArgs e)
        {
            Process();
        }
    }
}
