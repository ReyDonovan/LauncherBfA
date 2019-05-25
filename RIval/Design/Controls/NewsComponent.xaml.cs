using Ignite.Core;
using Ignite.Core.Repositories;
using Ignite.Design.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Ignite.Design.Controls
{
    /// <summary>
    /// Логика взаимодействия для NewsComponent.xaml
    /// </summary>
    public partial class NewsComponent : System.Windows.Controls.UserControl
    {
        public NewsComponent()
        {
            ApplicationEnv.Instance.ApplyStatus(ApplicationStatus.Loading);

            InitializeComponent();

            ApplicationEnv.Instance.ApplyStatus(ApplicationStatus.Normal);
        }

        private void AddNews(List<NewsRepository> news)
        {
            int startPos = 8;
            int posVal = 288;

            news.AsParallel().ForAll((element) =>
            {
                System.Windows.Application.Current.Dispatcher.BeginInvoke((Action)delegate
                {
                    NewsControl control = new NewsControl();
                    control.Title.Text = element.Title;
                    control.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    control.Link = element.Link;

                    control.Margin = new Thickness(startPos, 15, 0, 0);
                    startPos += posVal;

                    Dispatcher.BeginInvoke((MethodInvoker)(() =>
                    {
                        NewsScrollGrid.Children.Add(control);
                    }));
                });
            });
        }

        private void ScrollNews_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scrollviewer = sender as ScrollViewer;
            if (e.Delta > 0)
                scrollviewer.LineLeft();
            else
                scrollviewer.LineRight();
            e.Handled = true;
        }
    }
}
