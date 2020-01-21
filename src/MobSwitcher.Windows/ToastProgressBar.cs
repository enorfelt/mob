using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Globalization;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace MobSwitcher.Windows
{
  public class ToastProgressBar
  {
    public ToastProgressBar(double maxTickCount)
    {
      MaxTickCount = maxTickCount;
    }

    public double MaxTickCount { get; private set; }

    public void Tick(double newTickCount)
    {
      var data = new NotificationData
      {
        SequenceNumber = 0
      };

      // Assign new values
      // Note that you only need to assign values that changed. In this example
      // we don't assign progressStatus since we don't need to change it
      var progressValue = newTickCount / MaxTickCount;
      var stringValue = progressValue.ToString("0.##", CultureInfo.InvariantCulture);
      data.Values["progressValue"] = stringValue;
      var minutes = MaxTickCount / 60;
      var minutesLeft = Math.Round((MaxTickCount - newTickCount) / 60, 1);
      data.Values["progressValueString"] = $"{minutesLeft}/{minutes} minutes";

      // Update the existing notification's data by using tag/group
      ToastNotificationManager.CreateToastNotifier(ToastProperties.AppId).Update(data, ToastProperties.Tag, ToastProperties.Group);
    }

    internal void ClearHistory()
    {
      ToastNotificationManager.History.Clear(ToastProperties.AppId);
    }

    internal void Show()
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
                    Text = "Mobing in progress..."
                },
                new AdaptiveProgressBar()
                {
                    Value = new BindableProgressBarValue("progressValue"),
                    ValueStringOverride = new BindableString("progressValueString"),
                    Status = new BindableString("progressStatus")
                }
            }
          }
        },
      };

      var doc = new XmlDocument();
      var content = toastContent.GetContent();
      doc.LoadXml(content);
      // Create the toast notification
      var toastNotif = new ToastNotification(doc);
      toastNotif.Tag = ToastProperties.Tag;
      toastNotif.Group = ToastProperties.Group;
      toastNotif.Data = new NotificationData();
      toastNotif.Data.Values["progressValue"] = "0";
      var minutes = MaxTickCount / 60;
      toastNotif.Data.Values["progressValueString"] = $"{minutes}/{minutes} minutes";
      toastNotif.Data.Values["progressStatus"] = "Time left...";

      // And send the notification
      ToastNotificationManager.CreateToastNotifier(ToastProperties.AppId).Show(toastNotif);

      //https://docs.microsoft.com/en-us/windows/uwp/design/shell/tiles-and-notifications/toast-progress-bar#elements-that-support-data-binding
    }
  }
}
