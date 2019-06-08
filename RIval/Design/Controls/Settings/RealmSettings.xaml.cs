using Ignite.Core;
using Ignite.Core.Components.Parameters;
using System;
using System.Collections.Generic;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UserControl = System.Windows.Controls.UserControl;

namespace Ignite.Design.Controls.Settings
{
    /// <summary>
    /// Логика взаимодействия для RealmSettings.xaml
    /// </summary>
    public partial class RealmSettings : UserControl
    {
        public Dictionary<int, string> Pathes = new Dictionary<int, string>();

        public RealmSettings()
        {
            InitializeComponent();

            Localize();
            LoadFromConfig();
        }

        private void LoadFromConfig()
        {
            AtaldazarPath_Box.Text = ParamMgr.Instance.GetGameFolder(1) == "NOT FOUND" ? "C:\\" : ParamMgr.Instance.GetGameFolder(1);
            MotherlodePath_Box.Text = ParamMgr.Instance.GetGameFolder(2) == "NOT FOUND" ? "C:\\" : ParamMgr.Instance.GetGameFolder(2);
        }

        private void Localize()
        {
            RealmSettings_Title.Content = LanguageMgr.Instance.ValueOf("RealmSettings_Title");
            AtalDazarPath_Title.Content = LanguageMgr.Instance.ValueOf("AtalDazarPath_Title");
            ChangeAtaldazar_Button.Content = LanguageMgr.Instance.ValueOf("ChangeAtaldazar_Button");
            MotherlodePath_Title.Content = LanguageMgr.Instance.ValueOf("MotherlodePath_Title");
            ChangeMotherLode_Button.Content = LanguageMgr.Instance.ValueOf("ChangeMotherLode_Button");
        }

        private void ChangeAtaldazar_Button_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Pathes.Add(1, dialog.SelectedPath);

                AtaldazarPath_Box.Text = dialog.SelectedPath;
            }
        }

        private void ChangeMotherLode_Button_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Pathes.Add(2, dialog.SelectedPath);

                MotherlodePath_Box.Text = dialog.SelectedPath;
            }
        }
    }
}
