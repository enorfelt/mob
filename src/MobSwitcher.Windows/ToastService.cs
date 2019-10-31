using MobSwitcher.Core.Services;
using System;
using Windows.UI.Notifications;

namespace MobSwitcher.Windows
{
    public class ToastService : IToastService
    {
        public void Toast(string message)
        {
            if (string.IsNullOrEmpty(message))
                throw new ArgumentNullException(nameof(message));

            var template = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText01);

            var textNodes = template.GetElementsByTagName("text");
            textNodes.Item(0).InnerText = message;

            var notifier = ToastNotificationManager.CreateToastNotifier("MobSwitcher");
            var notification = new ToastNotification(template);
            notifier.Show(notification);
        }

        public void Toast(string message1, string message2)
        {
            if (string.IsNullOrEmpty(message1))
                throw new ArgumentNullException(nameof(message1));

            if (string.IsNullOrEmpty(message2))
                throw new ArgumentNullException(nameof(message2));

            var template = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);

            var textNodes = template.GetElementsByTagName("text");
            textNodes.Item(0).InnerText = message1;
            textNodes.Item(1).InnerText = message2;

            var notifier = ToastNotificationManager.CreateToastNotifier("MobSwitcher");
            var notification = new ToastNotification(template);
            notifier.Show(notification);
        }
    }
}
