using Ignite.Design.Controls.MessageBox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
        private string ReportUrl { get; set; }
        private string ReportData { get; set; }


        public void ShowReportError(string code, string desc, string reportUrl, string data)
        {
            ReportUrl = reportUrl;
            ReportData = data;

            MessageBoxBuilder
                .Create()
                .SetData(MessageBoxType.Error, code, desc, true)
                .ChangeActionButton(true, LanguageMgr.Instance.ValueOf("MessageBox_ReportError_ReportButtonName"), Report)
                .Show();
        }

        public void ShowCriticalError(string code, string desc)
        {
            MessageBoxBuilder
                .Create()
                .SetData(MessageBoxType.Error, code, desc, false)
                .ChangeActionButton(false, "", null)
                .Show();
        }

        public void ShowWarning(string code, string desc)
        {
            MessageBoxBuilder
                .Create()
                .SetData(MessageBoxType.Warning, code, desc, false)
                .ChangeActionButton(false, "", null)
                .Show();
        }

        public void ShowSuccess(string code, string desc)
        {
            MessageBoxBuilder
                .Create()
                .SetData(MessageBoxType.Success, code, desc, false)
                .ChangeActionButton(false, "", null)
                .Show();
        }

        public MessageBoxBuilder Builder() => MessageBoxBuilder.Create();

        private void Report(object e, MouseButtonEventArgs args)
        {

        }
    }
}
