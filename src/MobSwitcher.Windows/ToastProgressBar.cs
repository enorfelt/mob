using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Globalization;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using System.Threading.Tasks;
using System.Threading;

namespace MobSwitcher.Windows
{
  public class ToastProgressBar
  {
    private readonly ToastNotifier toastNotifier;
    private bool isStopped = false;
    public ToastProgressBar(double durationInSeconds)
    {
      toastNotifier = ToastNotificationManager.CreateToastNotifier(ToastProperties.AppId);
      DurationInSeconds = durationInSeconds;
      SecondsRemaining = durationInSeconds;
    }

    public double DurationInSeconds { get; set; }
    public double SecondsRemaining { get; set; }

    public void Start()
    {
      ClearHistory();
      if (DurationInSeconds > 0)
      {
        ShowProgressBar();
      }
      while (!isStopped && SecondsRemaining > 0)
      {
        Thread.Sleep(1000);
        SecondsRemaining--;
        UpdateProgressBar();
      }

      ShowCompleted();

      if (isStopped)
      {
        ClearHistory();
      }
    }

    public void Stop()
    {
      isStopped = true;
    }

    private void ShowCompleted()
    {
      if (isStopped)
      {
        return;
      }

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
                    Text = "Time is up!"
                },
                new AdaptiveText()
                {
                    Text = "mob next OR mob done"
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
      Show(new ToastNotification(doc));
    }

    private void Show(ToastNotification notification)
    {
      notification.Tag = ToastProperties.Tag;
      notification.Group = ToastProperties.Group;
      toastNotifier.Show(notification);
    }

    private void UpdateProgressBar()
    {
      var data = new NotificationData
      {
        SequenceNumber = 0
      };

      var percentageCompleted = (DurationInSeconds - SecondsRemaining) / DurationInSeconds;
      var percentageCompletedString = percentageCompleted.ToString("0.##", CultureInfo.InvariantCulture);
      data.Values["progressValue"] = percentageCompletedString;
      var durationInMinutes = DurationInSeconds / 60;
      var minutesLeft = Math.Round((SecondsRemaining / 60.0), 1);
      data.Values["progressValueString"] = $"{minutesLeft}/{durationInMinutes} minutes";
      
      toastNotifier.Update(data, ToastProperties.Tag, ToastProperties.Group);
    }

    private static void ClearHistory()
    {
      ToastNotificationManager.History.Clear(ToastProperties.AppId);
    }

    private void ShowProgressBar()
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
        Duration = ToastDuration.Short
      };

      var doc = new XmlDocument();
      var content = toastContent.GetContent();
      doc.LoadXml(content);
      // Create the toast notification
      var toastNotif = new ToastNotification(doc);
      toastNotif.Data = new NotificationData();
      toastNotif.Data.Values["progressValue"] = "0";
      var durationInMinutes = DurationInSeconds / 60;
      toastNotif.Data.Values["progressValueString"] = $"{durationInMinutes}/{durationInMinutes} minutes";
      toastNotif.Data.Values["progressStatus"] = "Time left...";

      // And send the notification
      Show(toastNotif);
    }
  }
}
