using Ignite.Core;
using Ignite.Core.Components.Auth;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

                CurtainElement.Visibility = Visibility.Visible;
                Preloader.Visibility = Visibility.Visible;
            }
            else
            {
                EmailBox.IsEnabled = true;
                PasswordBox.IsEnabled = true;
                QuestionsSelector.IsEnabled = true;
                QuestionAnswer.IsEnabled = true;
                CreateAccount.IsEnabled = true;
                AlreadyExistsAccountButton.IsEnabled = true;

                CurtainElement.Visibility = Visibility.Hidden;
                Preloader.Visibility = Visibility.Hidden;
            }
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

            if(QuestionsSelector.SelectedIndex == -1)
            {
                MessageBox.Show("Надо выбрать вопрос", "", MessageBoxButton.OK, MessageBoxImage.Error);

                QuestionAnswer.Text = "";
                SetLoadingState(false);
            }
            else
            {
                var result = RegisterFacade.Instance.Attempt(EmailBox.Text, PasswordBox.Password, QuestionsSelector.SelectedIndex.ToString(), QuestionAnswer.Text);
                if(result.Code == Core.Components.Auth.Types.AuthResultEnum.Ok)
                {
                    result = AuthFacade.Instance.Attempt(EmailBox.Text, PasswordBox.Password);
                    if(result.Code == Core.Components.Auth.Types.AuthResultEnum.Ok)
                    {
                        AuthFacade.Instance.CreateUser();

                        WindowMgr.Instance.Run<AuthorizeWindow>((auth) =>
                        {
                            auth.OpenGates();
                        });
                    }
                    else
                    {
                        MessageBox.Show(result.Message, "", MessageBoxButton.OK, MessageBoxImage.Error);

                        SetLoadingState(false);
                    }
                }
                else
                {
                    MessageBox.Show(result.Message, "", MessageBoxButton.OK, MessageBoxImage.Error);

                    SetLoadingState(false);
                }
            }
        }
    }
}
