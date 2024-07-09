using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZeonStore.Services;

namespace ZeonStore.Desktop
{
    public class MessageBox : IMessageBox
    {
        public Task ShowInstallationError(Exception exception)
        {
            string title = Locales.Resources.InstallationErrorTitle;
            string message = string.Format(Locales.Resources.InstallationErrorMessage, exception)
                .Replace("\\n", "\n");
            return ShowErrorMessage(title, message);
        }

        public Task ShowUnknownInstallationError()
        {
            string title = Locales.Resources.InstallationErrorTitle;
            string message = Locales.Resources.InstallationUnknownErrorMessage;
            return ShowErrorMessage(title, message);
        }

        private Task ShowErrorMessage(string title, string message)
        {
            return MessageBoxManager.GetMessageBoxStandard(title, message, ButtonEnum.Ok, Icon.Error)
                .ShowAsync();
        }
    }
}
