using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using WebPingzor.Data;
using WebPingzor.Data.Models;

namespace WebPingzor.MonitorManagement;
public class MonitorService(PingzorDbProvider dbProvider)
{
  private readonly PingzorDbProvider _dbProvider = dbProvider;

  public async Task<List<HttpMonitor>> GetAll()
  {
    using var context = _dbProvider.Create();
    return await context.HttpMonitors.ToListAsync();
  }

  public async Task<HttpMonitor?> GetById(int id)
  {
    using var context = _dbProvider.Create();
    return await context.HttpMonitors.FindAsync(id);
  }

  public async Task<HttpMonitor> Create(string name, string url, int interval)
  {
    using var context = _dbProvider.Create();

    var alreadyExists = await context.HttpMonitors.AnyAsync(m => m.Name.ToLower() == name.ToLower());
    if (alreadyExists)
    {
      throw new ValidationException("Monitor with the same name already exists");
    }

    var monitor = new HttpMonitor { Name = name, Url = url, Interval = interval };
    context.HttpMonitors.Add(monitor);
    await context.SaveChangesAsync();

    return monitor;
  }

  public async Task Update(int id, string name, string url, int interval)
  {
    using var context = _dbProvider.Create();

    var monitor = await context.HttpMonitors.FindAsync(id);
    if (monitor == null)
    {
      throw new Exception("Monitor not found");
    }

    monitor.Name = name;
    monitor.Url = url;
    monitor.Interval = interval;

    await context.SaveChangesAsync();
  }
}
