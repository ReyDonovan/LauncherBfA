using RIval.Design.Graphics;
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

namespace RIval
{
    /// <summary>
    /// Логика взаимодействия для WaitWindow.xaml
    /// </summary>
    public partial class WaitWindow : Window
    {
        public WaitWindow()
        {
            InitializeComponent();
        }

        private void Image_MouseEnter(object sender, MouseEventArgs e)
        {
            EffectManager.Instance.SetDropShadow((Image)sender, Colors.White, 0.9f);
        }
        private void Image_MouseLeave(object sender, MouseEventArgs e)
        {
            (sender as Image).Effect = null;
        }
        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
