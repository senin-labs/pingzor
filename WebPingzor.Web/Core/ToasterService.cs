using AntDesign;
using WebPingzor.Core;

namespace WebPingzor.Web.Core;

internal sealed class ToasterService(NotificationService notificationService) : IToasterService
{
#pragma warning disable CS4014 // We don't want to wait ultil the notification to dissapear
  public void Success(string title, string message)
  {
    this.SuccessAsync(title, message);
  }
#pragma warning restore CS4014

  public async Task SuccessAsync(string title, string message)
  {
    await notificationService.Success(new NotificationConfig
    {
      Message = title,
      Description = message
    });
  }

#pragma warning disable CS4014 // We don't want to wait ultil the notification to dissapear
  public void Error(string title, string message)
  {
    this.ErrorAsync(title, message);
  }

  public async Task ErrorAsync(string title, string message)
  {
    await notificationService.Error(new NotificationConfig
    {
      Message = title,
      Description = message
    });
  }
}
