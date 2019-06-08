using Ignite.Core;
using Ignite.Core.Components.Auth;
using Ignite.Core.Components.Message;
using Ignite.Design.Controls.Auth;
using System;
using System.Windows;
using System.Windows.Input;

namespace Ignite
{
    /// <summary>
    /// Логика взаимодействия для AuthorizeWindow.xaml
    /// </summary>
    public partial class AuthorizeWindow : Window, IWindowDispatcher
    {
        public AuthorizeWindow()
        {
            InitializeComponent();

            WindowMgr.Instance.AddHosted(this);

            SetActiveComponent(new LoginComponent());
        }

        public void AppendLocale(LanguageMgr langMgr)
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

        public void Connected()
        {
            throw new NotImplementedException();
        }

        public void Disconnect()
        {
            throw new NotImplementedException();
        }

        public void Loading()
        {
            throw new NotImplementedException();
        }

        public void Normal()
        {
            
        }

        public void SetActiveComponent(UIElement elem)
        {
            Container.Children.Clear();

            Container.Children.Add(elem);
        }

        public UIElement GetActiveComponent()
        {
            return Container.Children[0];
        }

        public void SetPreloader(bool enabled)
        {
            Preloader.IsEnabled = enabled;
        }

        public void OpenGates()
        {
            var user = AuthMgr.Instance.GetUser();
            if (user.UserName != null)
            {
                WindowMgr.Instance.Run<MainWindow>((mw) =>
                {
                    mw.AppendUser(user.UserName);
                    mw.Show();
                });

                Close();
            }
            else
            {
                ((LoginComponent)GetActiveComponent()).SetLoadingState(false);

                MessageBoxMgr.Instance.ShowCriticalError("#18-934", "An error occured while parsing user data. Please try again later");
            }
        }

        public Window ToWindow()
        {
            return this;
        }

        private void CloseButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
