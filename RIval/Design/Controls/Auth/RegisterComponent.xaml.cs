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
    public partial class RegisterComponent : UserControl
    {
        public RegisterComponent()
        {
            InitializeComponent();

            StartUp();
            Localize();
        }

        private void StartUp()
        {
            for(int i = 1; i <= 6; i++)
            {
                if(i == 1)
                {
                    QuestionsSelector.Text = LanguageMgr.Instance.ValueOf($"Register_questions_{i}");
                }

                QuestionsSelector.Items.Add(LanguageMgr.Instance.ValueOf($"Register_questions_{i}"));
            }
        }

        private void Localize()
        {
            PrivacyLink.Content = LanguageMgr.Instance.ValueOf("Auth_PrivacyLink");
            EmailBox.Text = LanguageMgr.Instance.ValueOf("Auth_RegisterComponent_EmailBoxHelpText");
            QuestionAnswer.Text = LanguageMgr.Instance.ValueOf("Auth_RegisterComponent_QuestionsAnswerHelpText");
            CreateAccount.Content = LanguageMgr.Instance.ValueOf("Auth_RegisterComponent_CreateAccount");
            AlreadyExistsAccountButton.Content = LanguageMgr.Instance.ValueOf("Auth_RegisterComponent_AlreadyExistsAccountButton");
        }

        private void CloseButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Environment.Exit(0);
        }

        private void SetLoadingState(bool enabled)
        {
            if(enabled)
            {
                EmailBox.IsEnabled = false;
                PasswordBox.IsEnabled = false;
                QuestionsSelector.IsEnabled = false;
                QuestionAnswer.IsEnabled = false;
                CreateAccount.IsEnabled = false;
                AlreadyExistsAccountButton.IsEnabled = false;
            }
            else
            {
                EmailBox.IsEnabled = true;
                PasswordBox.IsEnabled = true;
                QuestionsSelector.IsEnabled = true;
                QuestionAnswer.IsEnabled = true;
                CreateAccount.IsEnabled = true;
                AlreadyExistsAccountButton.IsEnabled = true;
            }

            WindowMgr.Instance.Run<AuthorizeWindow>((authWindow) =>
            {
                authWindow.SetPreloader(enabled);
            });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            WindowMgr.Instance.Run<AuthorizeWindow>((authWindow) =>
            {
                authWindow.SetActiveComponent(new LoginComponent());
            });
        }

        private void CreateAccount_Click(object sender, RoutedEventArgs e)
        {
            SetLoadingState(true);
        }
    }
}
