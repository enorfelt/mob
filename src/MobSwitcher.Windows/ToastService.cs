using Microsoft.Toolkit.Uwp.Notifications;
using MobSwitcher.Core.Services;
using System;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace MobSwitcher.Windows;

public class ToastService : IToastService
{
  public void Toast(string message)
  {
    if (string.IsNullOrEmpty(message))
      throw new ArgumentNullException(nameof(message));

    var toastContent = new ToastContentBuilder()
      .AddText(message)
      .AddButton(new ToastButtonDismiss())
      .SetToastScenario(ToastScenario.Alarm)
      .SetToastDuration(ToastDuration.Long)
      .AddAudio(new ToastAudio 
      {
        Loop = true,
        Src = new Uri("ms-winsoundevent:Notification.Looping.Alarm4")
      });

    Show(toastContent.GetXml());
  }

  public void Toast(string message1, string message2)
  {
    if (string.IsNullOrEmpty(message1))
      throw new ArgumentNullException(nameof(message1));

    if (string.IsNullOrEmpty(message2))
      throw new ArgumentNullException(nameof(message2));


    var toastContent = new ToastContentBuilder()
      .AddText(message1)
      .AddText(message2)
      .AddButton(new ToastButtonDismiss())
      .SetToastScenario(ToastScenario.Alarm)
      .SetToastDuration(ToastDuration.Long)
      .AddAudio(new ToastAudio 
      {
        Loop = true,
        Src = new Uri("ms-winsoundevent:Notification.Looping.Alarm4")
      });

    Show(toastContent.GetXml());
  }

  private static void Show(XmlDocument template)
  {
    var notifier = ToastNotificationManagerCompat.CreateToastNotifier();
    var notification = new ToastNotification(template)
    {
      Tag = ToastProperties.Tag,
      Group = ToastProperties.Group
    };
    notifier.Show(notification);
  }
}

