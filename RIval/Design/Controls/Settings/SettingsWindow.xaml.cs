using Ignite.Core;
using Ignite.Core.Components.Message;
using Ignite.Core.Components.Parameters;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Ignite.Design.Controls.Settings
{
    public enum SettingsTabs
    {
        Realm = 1,
        Accounts = 2
    }

    /// <summary>
    /// Логика взаимодействия для SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private UserControl Active = null;

        public SettingsWindow()
        {
            InitializeComponent();

            Localize();

            SetActiveTab(GameSettingsTab, SettingsTabs.Realm);
        }

        private void Localize()
        {
            Title.Content = LanguageMgr.Instance.ValueOf("SettingsWindow_General_Title");
            AccountSettingsTabTitle.Text = LanguageMgr.Instance.ValueOf("SettingsWindow_General_AccountSettingsTitle");
            GameSettingsTabTitle.Text = LanguageMgr.Instance.ValueOf("SettingsWindow_General_GameSettingsTabTitle");
            ResetSettingsButton.Text = LanguageMgr.Instance.ValueOf("SettingsWindow_General_ResetSettingsButton");
            CancelButton.Content = LanguageMgr.Instance.ValueOf("SettingsWindow_General_CancelButton");
            AppendSettingsButton.Content = LanguageMgr.Instance.ValueOf("SettingsWindow_General_AppendSettingsButton");
        }

        private void DropAll()
        {
            GameSettingsTab.BorderThickness = new Thickness(0, 0, 0, 0);
            GameSettingsTab.BorderBrush = null;
            GameSettingsTab.Background = null;

            AccountSettings.BorderThickness = new Thickness(0, 0, 0, 0);
            AccountSettings.BorderBrush = null;
            AccountSettings.Background = null;
        }

        private void SetActiveTab(Border toActive, SettingsTabs tab)
        {
            UserControl element = null;

            if (tab == SettingsTabs.Accounts)
                element = new AccountSettings();
            else
                element = new RealmSettings();

            if(element != null)
            {
                DropAll();

                Container.Children.Clear();

                toActive.BorderThickness = new Thickness(5, 0, 0, 0);
                toActive.BorderBrush = new SolidColorBrush(Color.FromRgb(40, 139, 222));
                toActive.Background = new SolidColorBrush(Color.FromRgb(28, 29, 35));

                Active = element;
                Container.Children.Add(element);
            }
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SetActiveTab(GameSettingsTab, SettingsTabs.Realm);
        }

        private void TextBlock_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            SetActiveTab(AccountSettings, SettingsTabs.Accounts);
        }

        private void TextBlock_MouseEnter(object sender, MouseEventArgs e)
        {
            if(sender is TextBlock)
            {
                var tb = sender as TextBlock;
                tb.Foreground = new SolidColorBrush(Color.FromRgb(53, 110, 158));
            }
        }

        private void TextBlock_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is TextBlock)
            {
                var tb = sender as TextBlock;
                tb.Foreground = new SolidColorBrush(Color.FromRgb(40, 139, 222));
            }
        }

        private void CloseButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void ResetSettingsButton_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is TextBlock)
            {
                var tb = sender as TextBlock;
                tb.Foreground = new SolidColorBrush(Color.FromRgb(174, 28, 28));
            }
        }

        private void ResetSettingsButton_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is TextBlock)
            {
                var tb = sender as TextBlock;
                tb.Foreground = new SolidColorBrush(Color.FromRgb(131, 25, 25));
            }
        }

        private void ResetSettingsButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBoxMgr.Instance
                .Builder()
                .ChangeActionButton(true, LanguageMgr.Instance.ValueOf("SettingsWindow_General_ResetMBActionButton"), CancelAllSettings)
                .ChangePrimaryButton(true, LanguageMgr.Instance.ValueOf("SettingsWindow_General_ResetMBPrimaryButton"))
                .SetData(MessageBoxType.Warning, LanguageMgr.Instance.ValueOf("SettingsWindow_General_ResetMBHead"), LanguageMgr.Instance.ValueOf("SettingsWindow_General_ResetMBDesc"), false)
                .Show();
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

        private void CancelAllSettings(object sender, RoutedEventArgs e)
        {
            ParamMgr.Instance.ResetAllSettings();

            MessageBoxMgr.Instance.ShowSuccess(LanguageMgr.Instance.ValueOf("SettingsWindow_General_ResetMBHead"), LanguageMgr.Instance.ValueOf("SettingsWindow_General_ResetMBDescSuccess"));

            ApplicationEnv.Instance.Restart();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AppendSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            if(Active != null)
            {
                ParamMgr.Instance.Append(((RealmSettings)Active).Pathes);
            }

            WindowMgr.Instance.Run<MainWindow>((window) =>
            {
                window.RestartGameComponents();
            });

            Close();
        }
    }
}
