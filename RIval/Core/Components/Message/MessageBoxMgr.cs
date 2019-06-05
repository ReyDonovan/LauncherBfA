using Ignite.Design.Controls.MessageBox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MessageBox = Ignite.Design.Controls.MessageBox.MessageBox;

namespace Ignite.Core.Components.Message
{
    public enum MessageBoxType
    {
        Error = 1,
        Success = 2,
        Warning = 3
    }

    public class MessageBoxMgr : Singleton<MessageBoxMgr>
    {
        private MessageBox Box { get; set; }

        public void Show(MessageBoxType type, string errorcode, string desc, bool report = false, bool withExit = false)
        {
            Application.Current.Dispatcher.Invoke(delegate {

                if (Box != null)
                    Box = null;

                Box = new MessageBox();

                SetImage(type);

                if (!report)
                {
                    Box.EnableButtons(false, "", true, LanguageMgr.Instance.ValueOf("MessageBox_OkButton"));
                }
                else
                {
                    Box.EnableButtons(false, LanguageMgr.Instance.ValueOf("MessageBox_ReportButton"), true, LanguageMgr.Instance.ValueOf("MessageBox_OkButton"));
                }

                Box.SetData($"{errorcode}", desc, GetTitle(type), withExit);

                Box.ShowDialog();
            });
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

            Box.SetImage(image);
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
