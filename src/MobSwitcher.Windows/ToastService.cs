using MobSwitcher.Core.Services;
using System;
using System.IO;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace MobSwitcher.Windows
{
    public class ToastService : IToastService
    {
        public void Toast(string message)
        {
            if (string.IsNullOrEmpty(message))
                throw new ArgumentNullException(nameof(message));

            var template = CreateToast(ToastTemplateType.ToastImageAndText01);

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

            var template = CreateToast(ToastTemplateType.ToastImageAndText02);

            var textNodes = template.GetElementsByTagName("text");
            textNodes.Item(0).InnerText = message1;
            textNodes.Item(1).InnerText = message2;
            
            var notifier = ToastNotificationManager.CreateToastNotifier("MobSwitcher");
            var notification = new ToastNotification(template);
            notifier.Show(notification);
        }

        public static XmlDocument CreateToast(ToastTemplateType toastTemplateType)
        {
            var template = ToastNotificationManager.GetTemplateContent(toastTemplateType);

            var toastNode = template.SelectSingleNode("/toast") as XmlElement;
            toastNode.SetAttribute("duration", "long");


            var audio = template.CreateElement("audio");
            audio.SetAttribute("src", "ms-winsoundevent:Notification.Looping.Alarm4");
            audio.SetAttribute("loop", "true");
            toastNode.AppendChild(audio);
            
            var imageNodes = template.GetElementsByTagName("image");
            if (imageNodes.Count > 0)
            {
                var imageElement = imageNodes[0] as XmlElement;
                if (imageElement != null)
                {
                    var imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "toast-icon.png");
                    imageElement.SetAttribute("src", imagePath);
                    imageElement.SetAttribute("alt", "mobswitcher logo");
                }
            }

            return template;
        }
    }
}
