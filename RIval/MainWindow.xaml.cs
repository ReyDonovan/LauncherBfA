using RIval.Core;
using RIval.Core.Components;
using RIval.Core.Components.Api;
using RIval.Core.Components.Update;
using RIval.Design.Controls;
using RIval.Design.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace RIval
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IWindowDispatcher
    {
        public MainWindow()
        {
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

            ApplicationEnv.Instance.SetLocale(LanguageMgr.Instance.FromConfig());
            LanguageMgr.Instance.Boot(LanguageMgr.Instance.FromConfig());


            Ataldazar_Button_Click(Ataldazar_Button, null);
            VersionLabel.Text = $"ignite.l.v {ApplicationEnv.Instance.AppVersion}";
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
            EffectManager.Instance.ChangeTextBlockColor((TextBlock)sender, Colors.Gray);
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
            SettingsButton.Effect = null;
            Ataldazar_Button.Effect = null;
            Motherlode_Button.Effect = null;

            ProductsButton.Effect = null;
            ProductsButton.MouseEnter -= TextBlock_MouseEnter;
            ProductsButton.MouseLeave -= TextBlock_MouseLeave;
            ProductsButton.MouseEnter += TextBlock_MouseEnter;
            ProductsButton.MouseLeave += TextBlock_MouseLeave;
            ProductsButton.Foreground = new SolidColorBrush(Colors.Gray);

            MagazineButton.Effect = null;
            MagazineButton.MouseEnter -= TextBlock_MouseEnter;
            MagazineButton.MouseLeave -= TextBlock_MouseLeave;
            MagazineButton.MouseEnter += TextBlock_MouseEnter;
            MagazineButton.MouseLeave += TextBlock_MouseLeave;
            MagazineButton.Foreground = new SolidColorBrush(Colors.Gray);
        }

        private void ProductsButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SetActive((FrameworkElement)sender);

            Process.Start(Core.Components.Api.ApiFacade.Instance.GetUri("shop-link"));
        }
        private void MagazineButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SetActive((FrameworkElement)sender);

            Process.Start(Core.Components.Api.ApiFacade.Instance.GetUri("acp-link"));
        }

        private void ForumButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SetActive((FrameworkElement)sender);

            Process.Start(Core.Components.Api.ApiFacade.Instance.GetUri("forum-link"));
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

        private void ContextMenu_AccountManageDown(object sender, MouseEventArgs e)
        {

        }

        private void ContextMenu_SupportServiceDown(object sender, MouseEventArgs e)
        {

        }

        private void ContextMenu_SettingsDown(object sender, MouseEventArgs e)
        {

        }

        private void ContextMenu_AboutUpdateDown(object sender, MouseEventArgs e)
        {

        }

        private void ContextMenu_SendFeedbackDown(object sender, MouseEventArgs e)
        {

        }

        private void ContextMenu_ExitFromAccountDown(object sender, MouseEventArgs e)
        {
            
        }

        public void SwitchMenuButtons(bool enable)
        {
            if(enable)
            {
                Ataldazar_Button.IsEnabled = true;
                Motherlode_Button.IsEnabled = true;
                SettingsButton.IsEnabled = true;
            }
            else
            {
                Ataldazar_Button.IsEnabled = false;
                Motherlode_Button.IsEnabled = false;
                SettingsButton.IsEnabled = false;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://sierra-rp.ru/forum");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Process.Start("https://sierra-rp.ru");
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
    }
}
