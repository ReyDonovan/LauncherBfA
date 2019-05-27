using Ignite.Core;
using Ignite.Design.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Ignite
{
    /// <summary>
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : Window, IWindowDispatcher
    {
        public Login()
        {
            InitializeComponent();

            WindowMgr.Instance.AddHosted(this);
        }

        private void IXButton_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Image)sender).Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/logo_mini_white.png"));
        }

        private void IXButton_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Image)sender).Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/logo_mini.png"));
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Environment.Exit(0);
        }

        private void ProgressBarGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (e.ChangedButton == MouseButton.Left)
                    this.DragMove();
            }
            catch (Exception) { }
        }

        private void TextBlock_MouseEnter(object sender, MouseEventArgs e)
        {
            ((TextBlock)sender).Foreground = new SolidColorBrush(Color.FromArgb(100, 207, 100, 11));
        }

        private void TextBlock_MouseLeave(object sender, MouseEventArgs e)
        {
            ((TextBlock)sender).Foreground = new SolidColorBrush(Colors.White);
        }

        private void TextBox_MouseEnter(object sender, MouseEventArgs e)
        {
            EffectManager.Instance.SetDropShadow((TextBox)sender, Color.FromArgb(100, 207, 100, 11), 0.3f);
        }

        private void TextBox_MouseLeave(object sender, MouseEventArgs e)
        {
            ((TextBox)sender).BorderBrush = new SolidColorBrush(Color.FromArgb(100, 207, 100, 11));
            ((TextBox)sender).Effect = null;
        }

        private void PasswordBox_MouseEnter(object sender, MouseEventArgs e)
        {
            EffectManager.Instance.SetDropShadow((PasswordBox)sender, Color.FromArgb(100, 207, 100, 11), 0.3f);
        }

        private void PasswordBox_MouseLeave(object sender, MouseEventArgs e)
        {
            ((PasswordBox)sender).BorderBrush = new SolidColorBrush(Color.FromArgb(100, 207, 100, 11));
            ((PasswordBox)sender).Effect = null;
        }

        private void LangSwitcher_Russian(object sender, RoutedEventArgs e)
        {
            SettingsMgr.Instance.WriteValue("user-language", "russian");
            SettingsMgr.Instance.Save();

            ApplicationEnv.Instance.Restart();
        }

        private void LangSwitcher_English(object sender, RoutedEventArgs e)
        {
            SettingsMgr.Instance.WriteValue("user-language", "english");
            SettingsMgr.Instance.Save();

            ApplicationEnv.Instance.Restart();
        }

        void IWindowDispatcher.Loading()
        {
            throw new NotImplementedException();
        }

        void IWindowDispatcher.Disconnect()
        {
            throw new NotImplementedException();
        }

        void IWindowDispatcher.Connected()
        {
            throw new NotImplementedException();
        }

        void IWindowDispatcher.Banned()
        {
            throw new NotImplementedException();
        }

        void IWindowDispatcher.Blocked()
        {
            throw new NotImplementedException();
        }

        void IWindowDispatcher.Normal()
        {
            throw new NotImplementedException();
        }

        Window IWindowDispatcher.ToWindow()
        {
            throw new NotImplementedException();
        }

        public void AppendLocale(LanguageMgr langMgr)
        {
            throw new NotImplementedException();
        }
    }
}
