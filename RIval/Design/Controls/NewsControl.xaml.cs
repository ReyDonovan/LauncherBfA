using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Ignite.Design.Controls
{
    /// <summary>
    /// Логика взаимодействия для ProductControl.xaml
    /// </summary>
    public partial class NewsControl : UserControl
    {
        public string Link { get; set; }

        public NewsControl()
        {
            InitializeComponent();

            NewsBg.Visibility = Visibility.Hidden;
            Content.Visibility = Visibility.Hidden;
            Title.Visibility = Visibility.Hidden;
        }

        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            NewsBg.Visibility = Visibility.Visible;
            Content.Visibility = Visibility.Visible;
            Title.Visibility = Visibility.Visible;
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            NewsBg.Visibility = Visibility.Hidden;
            Content.Visibility = Visibility.Hidden;
            Title.Visibility = Visibility.Hidden;

            /*
            var ani = new ThicknessAnimation();
            ani.By = new Thickness(0, 100, 0, 0);
            ani.Duration = new Duration(TimeSpan.FromSeconds(0.1));
            NewsBg.BeginAnimation(MarginProperty, ani);
            */
        }

        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.ChangedButton == MouseButton.Right)
                Process.Start(Link);
        }
    }
}
