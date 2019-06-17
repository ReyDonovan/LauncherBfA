using Ignite.Core;
using Ignite.Core.Components.Auth;
using Ignite.Core.Components.Message;
using System;
using System.Threading.Tasks;
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

            if(AuthMgr.Instance.LoadUser() != null)
            {
                WindowMgr.Instance.Run<AuthorizeWindow>((auth) =>
                {
                    auth.OpenGates();
                });
            }

            SetLoadingState(false);
        }

        private void Localize()
        {
            AuthLoginTitle.Content = LanguageMgr.Instance.ValueOf("Auth_LoginComponent_Title");
            EmailHelpText.Content = LanguageMgr.Instance.ValueOf("Auth_LoginComponent_EmailHelpText");
            PasswordHelpText.Content = LanguageMgr.Instance.ValueOf("Auth_LoginComponent_PasswordHelpText");
            LoginButton.Content = LanguageMgr.Instance.ValueOf("Auth_LoginComponent_LoginButton");
            RegisterButton.Content = LanguageMgr.Instance.ValueOf("Auth_LoginComponent_RegisterButton");
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

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            SetLoadingState(true);

            var email = EmailBox.Text;
            var password = PasswordBox.Password;

            var result = await AuthMgr.Instance.LoginAsync(email, password);

            if (result.Code == Core.Components.Auth.Types.AuthResultEnum.Ok)
            {
                AuthMgr.Instance.SaveUser(result.Token);

                await AuthMgr.Instance.LoadUserAsync();

                WindowMgr.Instance.Run<AuthorizeWindow>((auth) =>
                {
                    auth.OpenGates();
                });
            }
            else
            {
                MessageBoxMgr.Instance.ShowCriticalError("#18-754", LanguageMgr.Instance.ValueOf(result.Message));

                PasswordBox.Password = "";

                SetLoadingState(false);
            }
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
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

        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            
        }
    }
}
