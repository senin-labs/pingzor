using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WebPingzor.Data;
using WebPingzor.Data.Models;

namespace WebPingzor.MonitorBackgroundWorkers;

internal class HttpMonitorWorker(ILogger<HttpMonitorWorker> _logger, IServiceScopeFactory _scopeFactory) : BackgroundService
{
  protected override async Task ExecuteAsync(CancellationToken cancelToken)
  {
    _logger.LogInformation("HttpMonitorWorker is starting ...");

    while (!cancelToken.IsCancellationRequested)
    {
      try
      {
        var checkCount = await LoopIteration(cancelToken);

        if (checkCount == 0)
        {
          await Task.Delay(5000, cancelToken);
        }
      }
      catch (TaskCanceledException)
      {
        _logger.LogInformation("HttpMonitorWorker is stopping ...");
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, ex.InnerException?.Message ?? ex.Message);
        await Task.Delay(5000, cancelToken);
      }
    }
  }

  private async Task<int> LoopIteration(CancellationToken cancelToken)
  {
    using var scope = _scopeFactory.CreateScope();
    var dbProvider = scope.ServiceProvider.GetRequiredService<PingzorDbProvider>();
    using var db = dbProvider.Create();

    var monitors = await db.HttpMonitors.Where(m => m.NextCheck < DateTime.Now).OrderBy(m => m.NextCheck).Take(5).ToListAsync(cancelToken);

    for (var i = 0; i < monitors.Count; i++)
    {
      var monitor = monitors[i];
      var (statusCode, latency) = await CheckWebsite(monitor.Url, cancelToken);
      _logger.LogInformation($"{monitor.Name}: {statusCode} ({latency}ms)");

      var isOnline = statusCode >= 200 && statusCode < 300;

      monitor.NextCheck = DateTime.Now.AddSeconds(monitor.Interval);
      monitor.IsOnline = isOnline;

      var check = new MonitorStatusCheck
      {
        MonitorId = monitor.Id,
        StatusCode = statusCode,
        IsOnline = isOnline,
        Latency = latency,
        CheckedAt = DateTime.Now
      };
      db.MonitorStatusChecks.Add(check);
      await db.SaveChangesAsync(cancelToken);
    }

    return monitors.Count;
  }

  private async Task<(int statusCode, int latency)> CheckWebsite(string url, CancellationToken cancelToken)
  {
    var sw = Stopwatch.StartNew();
    var statusCode = await DoRequest(url, cancelToken);
    sw.Stop();
    return (statusCode, (int)sw.ElapsedMilliseconds);
  }

  private async Task<int> DoRequest(string url, CancellationToken cancelToken)
  {
    var _httpClient = new HttpClient();
    _httpClient.Timeout = TimeSpan.FromSeconds(10);

    try
    {
      var response = await _httpClient.GetAsync(url, cancelToken);
      return (int)response.StatusCode;
    }
    catch (HttpRequestException ex)
    {
      if (ex.StatusCode.HasValue)
      {
        return (int)ex.StatusCode.Value;
      }
      else
      {
        return 0;
      }
    }
  }
}
