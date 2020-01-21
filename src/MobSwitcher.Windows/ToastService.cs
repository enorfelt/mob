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
    public void Toast(string message)
    {
      if (string.IsNullOrEmpty(message))
        throw new ArgumentNullException(nameof(message));

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
                    Text = message
                }
            }
          }
        },
        Actions = new ToastActionsCustom()
        {
          Buttons =
          {
            new ToastButtonDismiss()
          }
        },
        Scenario = ToastScenario.Alarm,
        Duration = ToastDuration.Long,
        Audio = new ToastAudio
        {
          Loop = true,
          Src = new Uri("ms-winsoundevent:Notification.Looping.Alarm4")
        }
      };

      var doc = new XmlDocument();
      doc.LoadXml(toastContent.GetContent());
      Show(doc);
    }

    public void Toast(string message1, string message2)
    {
      if (string.IsNullOrEmpty(message1))
        throw new ArgumentNullException(nameof(message1));

      if (string.IsNullOrEmpty(message2))
        throw new ArgumentNullException(nameof(message2));


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
                    Text = message1
                },
                new AdaptiveText()
                {
                    Text = message2
                }
            }
          }
        },
        Actions = new ToastActionsCustom()
        {
          Buttons =
          {
            new ToastButtonDismiss()
          }
        },
        Scenario = ToastScenario.Alarm,
        Duration = ToastDuration.Long,
        Audio = new ToastAudio
        {
          Loop = true,
          Src = new Uri("ms-winsoundevent:Notification.Looping.Alarm4")
        }
      };

      var doc = new XmlDocument();
      doc.LoadXml(toastContent.GetContent());
      Show(doc);
    }

    private void Show(XmlDocument template)
    {
      var notifier = ToastNotificationManager.CreateToastNotifier(ToastProperties.AppId);
      var notification = new ToastNotification(template);
      notification.Tag = ToastProperties.Tag;
      notification.Group = ToastProperties.Group;
      notifier.Show(notification);
    }
  }
}
