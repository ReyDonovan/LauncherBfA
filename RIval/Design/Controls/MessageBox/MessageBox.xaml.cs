using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Ignite.Design.Controls.MessageBox
{
    /// <summary>
    /// Логика взаимодействия для MessageBox.xaml
    /// </summary>
    public partial class MessageBox : Window
    {
        private bool WithExit { get; set; }

        public MessageBox()
        {
            InitializeComponent();
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Environment.Exit(0);
        }

        public void SetImage(string path)
        {
            MessageBoxTypeIcon.Source = new BitmapImage(new Uri(path));
        }

        public void SetData(string errorcode, string errordesc, string title, bool withExit)
        {
            Title.Content = title;
            Title.FontSize = 24.0;

            ErrorCode.Content = errorcode;
            ErrorDescription.AppendText(errordesc);
            ErrorDescription.IsReadOnly = true;

            WithExit = withExit;
        }

        public void EnableButtons(bool blue = true, string blueVal = "REPORT", RoutedEventHandler blueact = null, bool red = true, string redVal = "OK")
        {
            if(!blue)
            {
                ReportButton.Visibility = Visibility.Hidden;
            }
            else
            {
                ReportButton.Content = blueVal;
                ReportButton.Click += blueact;
            }

            if(!red)
            {
                OkButton.Visibility = Visibility.Hidden;
            }
            else
            {
                OkButton.Content = redVal;
            }
        }

        public void SetPrimaryButton(bool enabled, string name)
        {
            if (!enabled)
            {
                OkButton.Visibility = Visibility.Hidden;
            }
            else
            {
                OkButton.Content = name;
            }
        }

        public void SetActionButton(bool enabled, string name, MouseButtonEventHandler handler)
        {
            if(enabled)
            {
                ReportButton.Visibility = Visibility.Visible;
                ReportButton.Content = name;
                ReportButton.MouseDown += handler;
            }
            else
            {
                ReportButton.Visibility = Visibility.Hidden;
            }
        }

        private void CheckButton_Click(object sender, RoutedEventArgs e)
        {
            if (WithExit)
                Environment.Exit(0);
            else
                Close();
        }

        private void Window_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (e.ChangedButton == MouseButton.Left)
                    this.DragMove();
            }
            catch (Exception) { }
        }
    }
}
