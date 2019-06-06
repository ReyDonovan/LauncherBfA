using Ignite.Design.Controls.MessageBox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MessageBox = Ignite.Design.Controls.MessageBox.MessageBox;

namespace Ignite.Core.Components.Message
{
    public class MessageBoxBuilder
    {
        private MessageBox BuildedBox { get; set; } = new MessageBox();

        public static MessageBoxBuilder Create()
        {
            return new MessageBoxBuilder();
        }

        public MessageBoxBuilder ChangePrimaryButton(bool enable, string name)
        {
            BuildedBox.SetPrimaryButton(enable, name);

            return this;
        }

        public MessageBoxBuilder ChangeActionButton(bool enable, string name, MouseButtonEventHandler handler)
        {
            BuildedBox.SetActionButton(enable, name, handler);

            return this;
        }

        public MessageBoxBuilder SetData(MessageBoxType type, string errorcode, string desc, bool withExit = false)
        {
            SetImage(type);

            BuildedBox.SetData(errorcode, desc, GetTitle(type), withExit);

            return this;
        }

        public void Show()
        {
            Application.Current.Dispatcher.Invoke(delegate
            {
                if (BuildedBox != null)
                    BuildedBox.ShowDialog();
            });
        }

        public MessageBox GetBuildedBox()
        {
            return BuildedBox;
        }

        private void SetImage(MessageBoxType type)
        {
            var image = type switch
            {
                MessageBoxType.Error => "pack://application:,,,/Resources/Icons/MessageBox/mb-icon-error.png",
                MessageBoxType.Success => "pack://application:,,,/Resources/Icons/MessageBox/mb-icon-success.png",
                MessageBoxType.Warning => "pack://application:,,,/Resources/Icons/MessageBox/mb-icon-warning.png",
                _ => "",
            };

            BuildedBox.SetImage(image);
        }

        private string GetTitle(MessageBoxType type)
        {
            var title = type switch
            {
                MessageBoxType.Error => LanguageMgr.Instance.ValueOf("MessageBox_Title_Error"),
                MessageBoxType.Success => LanguageMgr.Instance.ValueOf("MessageBox_Title_Success"),
                MessageBoxType.Warning => LanguageMgr.Instance.ValueOf("MessageBox_Title_Warning"),
                _ => "",
            };

            return title;
        }
    }
}
