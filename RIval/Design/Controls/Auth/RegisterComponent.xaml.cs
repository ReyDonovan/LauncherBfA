using Ignite.Core;
using Ignite.Core.Components.Auth;
using System;
using System.Drawing;
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
            LoginBox.Text = LanguageMgr.Instance.ValueOf("Auth_RegisterComponent_LoginBoxHelpText");
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

        private void SetErrorBorder(Control element, string errorText)
        {
            element.BorderBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 155, 45, 48));
            if(element is TextBox)
            {
                ((TextBox)element).Text = errorText;
            }
        }
        private void DropErrors()
        {
            var defbrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 129, 129, 129));
            EmailBox.BorderBrush = defbrush;
            LoginBox.BorderBrush = defbrush;
            PasswordBox.BorderBrush = defbrush;
            QuestionsSelector.BorderBrush = defbrush;
            QuestionAnswer.BorderBrush = defbrush;

            //Localize();
        }

        private async void CreateAccount_Click(object sender, RoutedEventArgs e)
        {
            SetLoadingState(true);

            DropErrors();

            if(QuestionsSelector.SelectedIndex == -1)
            {
                SetErrorBorder(QuestionAnswer, "Требуется ответ на вопрос");

                SetLoadingState(false);
            }
            else
            {
                var result = await AuthMgr.Instance.RegisterAsync(LoginBox.Text, EmailBox.Text, PasswordBox.Password, QuestionsSelector.SelectedIndex.ToString(), QuestionAnswer.Text);
                if (result.Code == Core.Components.Auth.Types.AuthResultEnum.Ok)
                {
                    result = await AuthMgr.Instance.LoginAsync(EmailBox.Text, PasswordBox.Password);
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
                        MessageBox.Show(LanguageMgr.Instance.ValueOf(result.Message), "", MessageBoxButton.OK, MessageBoxImage.Error);

                        SetLoadingState(false);
                    }
                }
                else
                {
                    MessageBox.Show(LanguageMgr.Instance.ValueOf(result.Message), "", MessageBoxButton.OK, MessageBoxImage.Error);

                    SetLoadingState(false);
                }
            }
        }

        private void QuestionsSelector_ContextMenuClosing(object sender, ContextMenuEventArgs e)
        {
            var cb = (ComboBox)sender;
            if(cb.SelectedIndex > -1)
            {
                cb.Text = cb.Text.Remove(20).Insert(cb.Text.Length, "...");
            }
        }
    }
}
