using Ignite.Core;
using Ignite.Core.Components.Auth;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Ignite.Design.Controls.Auth
{
    /// <summary>
    /// Логика взаимодействия для LoginComponent.xaml
    /// </summary>
    public partial class LoginComponent : UserControl
    {
        public LoginComponent()
        {
            InitializeComponent();

            Localize();

            SetLoadingState(true);

            if(AuthFacade.Instance.IsRemembered())
            {
                EmailBox.Text = AuthFacade.Instance.CurrentUser.Email;

                var result = AuthFacade.Instance.AttemptToken();

                SetLoadingState(false);

                if (result.Code == Core.Components.Auth.Types.AuthResultEnum.Ok)
                {
                    WindowMgr.Instance.Run<AuthorizeWindow>((auth) =>
                    {
                        auth.OpenGates();
                    });
                }
                else
                {
                    MessageBox.Show(LanguageMgr.Instance.ValueOf(result.Message), LanguageMgr.Instance.ValueOf("Auth_MessageBox_Title_Error"), MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                SetLoadingState(false);
            }
        }

        private void Localize()
        {
            AuthLoginTitle.Content = LanguageMgr.Instance.ValueOf("Auth_LoginComponent_Title");
            LoginButton.Content = LanguageMgr.Instance.ValueOf("Auth_LoginComponent_LoginButton");
            RegisterButton.Content = LanguageMgr.Instance.ValueOf("Auth_LoginComponent_RegisterButton");
            RecoveryPasswordLink.Content = LanguageMgr.Instance.ValueOf("Auth_LoginComponent_RecoveryPasswordLink");
            RememberMeLabel.Content = LanguageMgr.Instance.ValueOf("Auth_LoginComponent_RememberMeCheckBox");
        }

        public void SetLoadingState(bool loading)
        {
            if(loading)
            {
                CurtainElement.Visibility = Visibility.Visible;
                Preloader.Visibility = Visibility.Visible;
            }
            else
            {
                CurtainElement.Visibility = Visibility.Hidden;
                Preloader.Visibility = Visibility.Hidden;
            }
        }

        private void CloseButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            WindowMgr.Instance.Run<AuthorizeWindow>((authWindow) =>
            {
                authWindow.SetActiveComponent(new RegisterComponent());
            });
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            SetLoadingState(true);

            var email = EmailBox.Text;
            var password = PasswordBox.Password;

            var result = AuthFacade.Instance.Attempt(email, password);

            if (result.Code == Core.Components.Auth.Types.AuthResultEnum.Ok)
            {
                if(RememberCheckBox.IsChecked == true)
                    AuthFacade.Instance.CreateUser();

                WindowMgr.Instance.Run<AuthorizeWindow>((auth) =>
                {
                    auth.OpenGates();
                });
            }
            else
            {
                MessageBox.Show(result.Message, LanguageMgr.Instance.ValueOf("Auth_MessageBox_Title_Error"), MessageBoxButton.OK, MessageBoxImage.Error);

                EmailBox.Text = "";
                PasswordBox.Password = "";

                SetLoadingState(false);
            }
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            lock (new object())
            {
                LangSwitcher.ContextMenu.IsOpen = !LangSwitcher.ContextMenu.IsOpen;
            }
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
    }
}
