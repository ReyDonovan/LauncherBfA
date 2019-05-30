using Ignite.Core;
using Ignite.Core.Components;
using Ignite.Core.Components.Api;
using Ignite.Core.Components.Update;
using Ignite.Design.Controls;
using Ignite.Design.Graphics;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Ignite
{
    /// <summary>
    /// Логика взаимодействия для AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window, IWindowDispatcher
    {
        public AuthWindow()
        {
            InitializeComponent();
        }

        public void AppendLocale(LanguageMgr langMgr)
        {
            
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
            throw new NotImplementedException();
        }

        public Window ToWindow()
        {
            return this;
        }
    }
}
