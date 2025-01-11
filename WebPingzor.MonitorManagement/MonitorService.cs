using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using WebPingzor.Core;
using WebPingzor.Data;
using WebPingzor.Data.Models;

namespace WebPingzor.MonitorManagement;
public class MonitorService(
  PingzorDbProvider _dbProvider,
  IAuthService _authService)
{
  public async Task<List<T>> QueryMonitors<T>(Func<IQueryable<HttpMonitor>, IQueryable<T>> query)
  {
    var userId = await _authService.GetUserId();
    if (userId == null)
    {
      throw new ValidationException("User not found");
    }

    using var context = _dbProvider.Create();
    var userMonitors = context.HttpMonitors.Where(m => m.UserId == userId);
    return await query(userMonitors).ToListAsync();
  }

  public async Task<T> QueryMonitor<T>(Func<IQueryable<HttpMonitor>, IQueryable<T>> query)
  {
    var userId = await _authService.GetUserId();
    if (userId == null)
    {
      throw new ValidationException("User not found");
    }

    using var context = _dbProvider.Create();
    var userMonitors = context.HttpMonitors.Where(m => m.UserId == userId);
    var queryResult = query(userMonitors);
    return await queryResult.FirstOrDefaultAsync();
  }

  public async Task<HttpMonitor> Create(string name, string url, int interval)
  {
    var userId = await _authService.GetUserId();
    if (userId == null)
    {
      throw new ValidationException("User not found");
    }

    using var context = _dbProvider.Create();

    var alreadyExists = await context.HttpMonitors.AnyAsync(m => m.Name.ToLower() == name.ToLower() && m.UserId == userId);
    if (alreadyExists)
    {
      throw new ValidationException("Monitor with the same name already exists");
    }

    var monitor = new HttpMonitor
    {
      UserId = userId.Value,
      Name = name,
      Url = url,
      Interval = interval
    };
    context.HttpMonitors.Add(monitor);
    await context.SaveChangesAsync();

    return monitor;
  }

  public async Task Update(int id, string name, string url, int interval)
  {
    var userId = await _authService.GetUserId();
    using var context = _dbProvider.Create();

    var monitor = await context.HttpMonitors.FindAsync(id);
    if (monitor == null || monitor.UserId != userId)
    {
      throw new Exception("Monitor not found");
    }

    monitor.Name = name;
    monitor.Url = url;
    monitor.Interval = interval;

    await context.SaveChangesAsync();
  }
}
