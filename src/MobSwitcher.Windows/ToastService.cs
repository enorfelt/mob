using Microsoft.Toolkit.Uwp.Notifications;
using MobSwitcher.Core.Services;
using System;
using System.IO;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace MobSwitcher.Windows
{
  public class ToastService : IToastService
  {
    private const string tag = "mob-timer";
    private const string group = "mobswitcher";

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
      //toastNode.SetAttribute("scenario", "alarm");

      var audio = template.CreateElement("audio");
      audio.SetAttribute("src", "ms-winsoundevent:Notification.Looping.Alarm4");
      audio.SetAttribute("loop", "true");
      toastNode.AppendChild(audio);

      var actions = template.CreateElement("actions");
      var nextAction = template.CreateElement("action");
      nextAction.SetAttribute("content", "OK");
      nextAction.SetAttribute("arguments", "dismiss");
      nextAction.SetAttribute("activationType", "background");
      //nextAction.SetAttribute("imageUri", "Assets/ToastButtonIcons/Dismiss.png");
      //actions.AppendChild(nextAction);
      //toastNode.AppendChild(actions);

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

    public void ProgressBar(int maxCount)
    {
      var content = new ToastContent()
      {
        Visual = new ToastVisual()
        {
          BindingGeneric = new ToastBindingGeneric()
          {
            Children =
            {
              new AdaptiveText()
              {
                Text = "Downloading your weekly playlist..."
              },
              new AdaptiveProgressBar()
              {
                  Title = "Weekly playlist",
                  Value = new BindableProgressBarValue("progressValue"),
                  ValueStringOverride = new BindableString("progressValueString"),
                  Status = new BindableString("progressStatus")
              }
            }
          }
        }
      };

      var doc = new XmlDocument();
      doc.LoadXml(content.GetContent());
      var toast = new ToastNotification(doc);

      //https://docs.microsoft.com/en-us/windows/uwp/design/shell/tiles-and-notifications/toast-progress-bar#elements-that-support-data-binding
    }
  }
}
