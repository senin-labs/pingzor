namespace WebPingzor.Core;

public interface IToasterService
{
  void Success(string title, string message);
  Task SuccessAsync(string title, string message);

  void Error(string title, string message);
  Task ErrorAsync(string title, string message);
}
