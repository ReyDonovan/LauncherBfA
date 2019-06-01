using Ignite.Core;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

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
        }

        private void Localize()
        {
            AuthLoginTitle.Content = LanguageMgr.Instance.ValueOf("Auth_LoginComponent_Title");
            LoginButton.Content = LanguageMgr.Instance.ValueOf("Auth_LoginComponent_LoginButton");
            RegisterButton.Content = LanguageMgr.Instance.ValueOf("Auth_LoginComponent_RegisterButton");
            RecoveryPasswordLink.Content = LanguageMgr.Instance.ValueOf("Auth_LoginComponent_RecoveryPasswordLink");
            PrivacyLink.Content = LanguageMgr.Instance.ValueOf("Auth_PrivacyLink");
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
    }
}
