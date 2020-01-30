using Microsoft.Toolkit.Uwp.Notifications;
using MobSwitcher.Core.Services;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using static MobSwitcher.Windows.TaskbarProgress;

namespace MobSwitcher.Windows
{
  public class ToastProgressBar
  {
    private readonly ISayService sayService;
    private static ToastNotifier toastNotifier = ToastNotificationManager.CreateToastNotifier(ToastProperties.AppId);
    private bool isStopped = false;
    public ToastProgressBar(ISayService sayService, int durationInSeconds)
    {
      this.sayService = sayService;
      DurationInSeconds = durationInSeconds;
      TaskbarProgress.SetState(TaskbarState.Normal);
    }

    public int DurationInSeconds { get; set; }

    public void Start()
    {
      ClearHistory();
      if (DurationInSeconds > 0)
      {
        var timerState = new TimerState { SecondsRemaining = DurationInSeconds };
        using var timer = new Timer(
              callback: new TimerCallback(TimerTask),
              state: timerState,
              dueTime: 1000,
              period: 1000);
        ShowProgressBar();
        while (timerState.SecondsRemaining >= 0 && !isStopped)
        {
          Task.Delay(1000).Wait();
          UpdateProgressBar(timerState.SecondsRemaining);
        }
      }

      ShowCompleted();
      TaskbarProgress.SetState(TaskbarProgress.TaskbarState.NoProgress);
    }

    public void Stop()
    {
      isStopped = true;
      ClearHistory();
    }

    public static void ClearHistory()
    {
      ToastNotificationManager.History.Clear(ToastProperties.AppId);
    }

    private static void TimerTask(object timerState)
    {
      var state = timerState as TimerState;
      Interlocked.Decrement(ref state.SecondsRemaining);
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

    private static void Show(ToastNotification notification)
    {
      var settings = toastNotifier.Setting;
      if (settings != NotificationSetting.Enabled)
      {
        return;
      }

      notification.Tag = ToastProperties.Tag;
      notification.Group = ToastProperties.Group;
      toastNotifier.Show(notification);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "<Pending>")]
    private bool IsToastEnabled()
    {
      bool isEnabled;
      try
      {
        isEnabled = toastNotifier.Setting == NotificationSetting.Enabled;
      }
      catch (Exception ex)
      {
        isEnabled = true;
        this.sayService.SayError($"Problem to get toast settings. Probably for the first time. Try showing anyway. Reason: {ex.Message}");
      }

      return isEnabled;
    }

    private void UpdateProgressBar(int secondsRemaining)
    {
      var data = new NotificationData
      {
        SequenceNumber = 0
      };

      var percentageCompleted = (0.0 + DurationInSeconds - secondsRemaining) / DurationInSeconds;
      var percentageCompletedString = percentageCompleted.ToString("0.##", CultureInfo.InvariantCulture);
      data.Values["progressValue"] = percentageCompletedString;
      var durationInMinutes = DurationInSeconds / 60;
      var minutesLeft = Math.Round((secondsRemaining / 60.0), 1);
      data.Values["progressValueString"] = $"{minutesLeft}/{durationInMinutes} minutes";

      toastNotifier.Update(data, ToastProperties.Tag, ToastProperties.Group);

      TaskbarProgress.SetValue(percentageCompleted * 100, 100);
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

      var toastNotif = new ToastNotification(doc);
      toastNotif.Data = new NotificationData();
      toastNotif.Data.Values["progressValue"] = "0";
      var durationInMinutes = DurationInSeconds / 60;
      toastNotif.Data.Values["progressValueString"] = $"{durationInMinutes}/{durationInMinutes} minutes";
      toastNotif.Data.Values["progressStatus"] = "Time left...";

      Show(toastNotif);
    }

    private class TimerState
    {
      public int SecondsRemaining;
    }
  }
}
