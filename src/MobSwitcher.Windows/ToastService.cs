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
      var toastContent = new ToastContent()
      {
        Visual = new ToastVisual()
        {
          BindingGeneric = new ToastBindingGeneric()
          {
            Children =
            {
                new AdaptiveText()
                {
                    Text = "Downloading this week's new music..."
                },
                new AdaptiveProgressBar()
                {
                    Value = new BindableProgressBarValue("progressValue"),
                    ValueStringOverride = new BindableString("progressValueString"),
                    Title = new BindableString("progressTitle"),
                    Status = new BindableString("progressStatus")
                }
            }
          }
        },
        Actions = new ToastActionsCustom()
        {
          Buttons =
        {
            new ToastButton("Pause", "action=pauseDownload&downloadId=9438108")
            {
                ActivationType = ToastActivationType.Background
            },
            new ToastButton("Cancel", "action=cancelDownload&downloadId=9438108")
            {
                ActivationType = ToastActivationType.Background
            }
        }
        },
        Launch = "action=viewDownload&downloadId=9438108"
      };



      var doc = new XmlDocument();
      var content = toastContent.GetContent();
      doc.LoadXml(content);
      // Create the toast notification
      var toastNotif = new ToastNotification(doc);
      toastNotif.Data = new NotificationData();
      toastNotif.Data.Values["progressTitle"] = "Katy Perry";
      toastNotif.Data.Values["progressValue"] = "0.6";
      toastNotif.Data.Values["progressValueString"] = "15/26 songs";
      toastNotif.Data.Values["progressStatus"] = "Downloading...";
      var appId = "enorfelt!MobSwitcher";
      // And send the notification
      ToastNotificationManager.CreateToastNotifier(appId).Show(toastNotif);

      //https://docs.microsoft.com/en-us/windows/uwp/design/shell/tiles-and-notifications/toast-progress-bar#elements-that-support-data-binding
    }
  }
}
