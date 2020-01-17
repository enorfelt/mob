namespace MobSwitcher.Core.Services
{
  public interface IToastService
  {
    void Toast(string message);
    void Toast(string message1, string message2);
    void ProgressBar(int maxCount);
  }
}
