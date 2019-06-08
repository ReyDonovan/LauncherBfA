using Ignite.Core;
using Ignite.Core.Components;
using Ignite.Core.Components.Api;
using Ignite.Core.Components.Auth;
using Ignite.Core.Components.Message;
using Ignite.Core.Components.Update;
using Ignite.Design.Controls;
using Ignite.Design.Controls.Settings;
using Ignite.Design.Graphics;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Ignite
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IWindowDispatcher
    {
        public MainWindow()
        {
            if(!ApplicationEnv.Instance.IsUserAdministrator())
            {
                MessageBoxMgr.Instance.ShowCriticalError(LanguageMgr.Instance.ValueOf("MainWindow_Init_RunAs_Header"), LanguageMgr.Instance.ValueOf("MainWindow_Init_RunAs_Desc"));

                Environment.Exit(0);
            }

            InitializeComponent();

            Logger.Instance.WriteLine($"Application booted in: {DateTime.Now.ToFileTimeUtc()}", LogLevel.Info);

            WindowMgr.Instance.AddHosted(this);

            SettingsMgr.Instance.Start(Core.Settings.SettingsDriver.File);
            var temp = ApplicationEnv.Instance.AppVersion;
            SettingsMgr.Instance.WriteOption(new Core.Settings.Option("version", $"{temp.Major}.{temp.Minor}.{temp.Resolution}.{temp.Prefix}"));
            SettingsMgr.Instance.Save();
            if (UpdateFacade.Instance.IsUpdateNeeded())
            {
                UpdateFacade.Instance.StartUpdate();
            }

            ApplicationEnv.Instance.SetLocale(LanguageMgr.Instance.GetCurrentLang());

            this.Hide();
            AuthorizeWindow auth = new AuthorizeWindow();
            auth.Show();

            Ataldazar_Button_Click(Ataldazar_Button, null);
            VersionLabel.Text = $"ignite.l.v {ApplicationEnv.Instance.AppVersion}";
        }

        public void RestartGameComponents()
        {
            WindowMgr.Instance.RemoveCachedGameComponent(1);
            WindowMgr.Instance.RemoveCachedGameComponent(2);

            Ataldazar_Button_Click(Ataldazar_Button, null);
        }

        public void AppendUser(string user)
        {
            UserProfileButton.Text = user.ToUpper();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (e.ChangedButton == MouseButton.Left)
                    this.DragMove();
            }
            catch (Exception) { }
        }

        private void Image_MouseEnter(object sender, MouseEventArgs e)
        {
            EffectManager.Instance.SetDropShadow((Image)sender, Colors.White, 0.4f);
        }
        private void Image_MouseLeave(object sender, MouseEventArgs e)
        {
            (sender as Image).Effect = null;
        }
        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Environment.Exit(0);
        }

        private void TextBlock_MouseEnter(object sender, MouseEventArgs e)
        {
            EffectManager.Instance.ChangeTextBlockColor((TextBlock)sender, Colors.White);
        }
        private void TextBlock_MouseLeave(object sender, MouseEventArgs e)
        {
            EffectManager.Instance.ChangeTextBlockColor((TextBlock)sender, Color.FromRgb(232, 228, 228));
        }

        private void Ataldazar_Button_Click(object sender, RoutedEventArgs e)
        {
            SetActive((FrameworkElement)sender);

            Page.Children.Clear();

            var component = WindowMgr.Instance.GetCachedGameComponent(1);
            if (component == null)
            {
                component = new GameComponent("ATAL'DAZAR", 1);
                WindowMgr.Instance.CacheGameComponent(component, 1);
            }

            Page.Children.Add(component);
        }
        private void Motherlode_Button_Click(object sender, RoutedEventArgs e)
        {
            SetActive((FrameworkElement)sender);

            Page.Children.Clear();

            var component = WindowMgr.Instance.GetCachedGameComponent(2);
            if (component == null)
            {
                component = new GameComponent("MOTHERLODE", 2);
                WindowMgr.Instance.CacheGameComponent(component, 2);
            }

            Page.Children.Add(component);
        }

        private void SetActive(FrameworkElement element)
        {
            DropAll();

            if(element is TextBlock)
            {
                ((TextBlock)element).Foreground = new SolidColorBrush(Colors.White);
            }

            EffectManager.Instance.SetDropShadow(element, Colors.White, 0.4f);

            element.MouseEnter -= TextBlock_MouseEnter;
            element.MouseLeave -= TextBlock_MouseLeave;
        }
        private void DropAll()
        {
            Ataldazar_Button.Effect = null;
            Motherlode_Button.Effect = null;

            MagazineButtonHeader.Effect = null;
            MagazineButtonHeader.MouseEnter -= TextBlock_MouseEnter;
            MagazineButtonHeader.MouseLeave -= TextBlock_MouseLeave;
            MagazineButtonHeader.MouseEnter += TextBlock_MouseEnter;
            MagazineButtonHeader.MouseLeave += TextBlock_MouseLeave;
            MagazineButtonHeader.Foreground = new SolidColorBrush(Color.FromRgb(232, 228, 228));

            ACPButtonHeader.Effect = null;
            ACPButtonHeader.MouseEnter -= TextBlock_MouseEnter;
            ACPButtonHeader.MouseLeave -= TextBlock_MouseLeave;
            ACPButtonHeader.MouseEnter += TextBlock_MouseEnter;
            ACPButtonHeader.MouseLeave += TextBlock_MouseLeave;
            ACPButtonHeader.Foreground = new SolidColorBrush(Color.FromRgb(232, 228, 228));

            ForumButtonHeader.Effect = null;
            ForumButtonHeader.MouseEnter -= TextBlock_MouseEnter;
            ForumButtonHeader.MouseLeave -= TextBlock_MouseLeave;
            ForumButtonHeader.MouseEnter += TextBlock_MouseEnter;
            ForumButtonHeader.MouseLeave += TextBlock_MouseLeave;
            ForumButtonHeader.Foreground = new SolidColorBrush(Color.FromRgb(232, 228, 228));
        }

        private void ProductsButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SetActive((FrameworkElement)sender);

            Process.Start(ApiFacade.Instance.GetUri("shop-link"));
        }
        private void MagazineButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SetActive((FrameworkElement)sender);

            Process.Start(ApiFacade.Instance.GetUri("acp-link"));
        }

        private void ForumButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SetActive((FrameworkElement)sender);

            Process.Start(ApiFacade.Instance.GetUri("forum-link"));
        }

        public void Loading()
        {
            Page.Visibility            = Visibility.Hidden;
        }
        public void Disconnect()
        {
            throw new NotImplementedException();
        }
        public void Connected()
        {
            throw new NotImplementedException();
        }
        public void Banned()
        {
            throw new NotImplementedException();
        }
        public void Blocked()
        {
            throw new NotImplementedException();
        }
        public void Normal()
        {
            Page.Visibility            = Visibility.Visible;
        }
        public Window ToWindow()
        {
            return this;
        }

        private void MinimizeButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(WindowState != WindowState.Minimized)
            {
                WindowState = WindowState.Minimized;
            }
        }

        private void IXButton_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Image)sender).Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/logo_basic_white.png"));
        }
        private void IXButton_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Image)sender).Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/logo_basic.png"));
        }
        private void IXButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                ((Image)sender).Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/logo_basic_white.png"));

                Image image = sender as Image;
                ContextMenu contextMenu = image.ContextMenu;
                contextMenu.PlacementTarget = image;
                contextMenu.IsOpen = true;
            }
        }

        public void SwitchMenuButtons(bool enable)
        {
            if(enable)
            {
                Ataldazar_Button.IsEnabled = true;
                Motherlode_Button.IsEnabled = true;
            }
            else
            {
                Ataldazar_Button.IsEnabled = false;
                Motherlode_Button.IsEnabled = false;
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            SetActive((FrameworkElement)sender);

            Page.Children.Clear();

            var component = WindowMgr.Instance.GetCachedGameComponent(2);
            if(component == null)
            {
                component = new GameComponent("BATTLE FOR AZEROTH", 2);
                WindowMgr.Instance.CacheGameComponent(component, 2);
            }

            Page.Children.Add(component);
        }

        private void Bugreport_Button_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(ApiFacade.Instance.GetUri("bug-report-link"));
        }

        public void AppendLocale(LanguageMgr langMgr)
        {
            //Language setting here
            MagazineButtonHeader.Text = langMgr.ValueOf(MagazineButtonHeader.Name);
            ACPButtonHeader.Text      = langMgr.ValueOf(ACPButtonHeader.Name);
            ForumButtonHeader.Text    = langMgr.ValueOf(ForumButtonHeader.Name);
            ServersLabel.Text         = langMgr.ValueOf(ServersLabel.Name);
            LinksLabel.Text           = langMgr.ValueOf(LinksLabel.Name);
            Bugreport_Button.Content  = langMgr.ValueOf(Bugreport_Button.Name);
            CloseTooltip.Content      = langMgr.ValueOf("Tooltip_CloseApp");
            MinimizeTooltip.Content   = langMgr.ValueOf("Tooltip_Minimise");
            SettingsMenuItem.Header   = langMgr.ValueOf("Tooltip_Settigs");
            LogoutMenuItem.Header     = langMgr.ValueOf("Tooltip_Logout");
        }

        private void MenuItem_LangSwitch_English_MouseDown(object sender, MouseButtonEventArgs e)
        {
            LanguageMgr.Instance.SetLang(Languages.English);
        }

        private void MenuItem_LangSwitch_Russian_MouseDown(object sender, MouseButtonEventArgs e)
        {
            LanguageMgr.Instance.SetLang(Languages.Russian);
        }

        private void MenuItem_LangSwitch_English_Click(object sender, RoutedEventArgs e)
        {
            LanguageMgr.Instance.SetLang(Languages.English);
        }

        private void MenuItem_LangSwitch_Russian_Click(object sender, RoutedEventArgs e)
        {
            LanguageMgr.Instance.SetLang(Languages.Russian);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            AuthMgr.Instance.Logout();
        }

        private void SettingsButtons_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
                SettingsButtons.ContextMenu.IsOpen = !SettingsButtons.ContextMenu.IsOpen;
        }

        private void UsernameBlock_MouseEnter(object sender, MouseEventArgs e)
        {
            Border ct = sender as Border;
            ct.BorderBrush = new SolidColorBrush(Color.FromRgb(232, 228, 228));
        }

        private void UsernameBlock_MouseLeave(object sender, MouseEventArgs e)
        {
            Border ct = sender as Border;
            ct.BorderBrush = new SolidColorBrush(Color.FromRgb(147, 147, 147));
        }

        private void SettingsButtons_MouseEnter(object sender, MouseEventArgs e)
        {
            UsernameBlock_MouseEnter(SettingsBlock, null);
        }

        private void SettingsButtons_MouseLeave(object sender, MouseEventArgs e)
        {
            UsernameBlock_MouseLeave(SettingsBlock, null);
        }

        private void Image_MouseEnter_1(object sender, MouseEventArgs e)
        {
            UsernameBlock_MouseEnter(LangBlock, null);
        }

        private void Image_MouseLeave_1(object sender, MouseEventArgs e)
        {
            UsernameBlock_MouseLeave(LangBlock, null);
        }

        private void SettingsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settings = new SettingsWindow();
            settings.ShowDialog();
        }
    }
}
