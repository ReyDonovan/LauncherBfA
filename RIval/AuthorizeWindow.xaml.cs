using Ignite.Core;
using Ignite.Core.Components.Auth;
using Ignite.Design.Controls.Auth;
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
using System.Windows.Shapes;

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

        public void SetPreloader(bool enabled)
        {
            Preloader.IsEnabled = enabled;
        }

        public void OpenGates()
        {
            WindowMgr.Instance.Run<MainWindow>((mw) =>
            {
                mw.AppendUser(AuthFacade.Instance.CurrentUser.UserName);
                mw.Show();
            });

            this.Hide();
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
