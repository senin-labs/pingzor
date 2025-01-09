using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WebPingzor.Data.Models;

namespace WebPingzor.Data;

public class UserService(IDbContextFactory<PingzorDbContext> contextFactory)
{
  private IDbContextFactory<PingzorDbContext> ContextFactory { get; } = contextFactory;

  public async Task<int> GetNumberOfUsers()
  {
    await Task.Delay(1000);
    using var context = ContextFactory.CreateDbContext();
    return await context.Users.CountAsync();
  }

  public async Task<User?> GetUserById(int id)
  {
    await Task.Delay(1000);
    using var context = ContextFactory.CreateDbContext();
    return await context.Users.FindAsync(id);
  }

  public async Task<User?> CreateUser(string name, string email)
  {
    await Task.Delay(1000);
    using var context = ContextFactory.CreateDbContext();
    var user = new User { Name = name, Email = email };
    context.Users.Add(user);
    await context.SaveChangesAsync();
    return user;
  }

  public async Task<List<User>> AllUsers()
  {
    using var context = ContextFactory.CreateDbContext();
    return await context.Users.ToListAsync();
  }

  public async Task<List<T>> AllUsers<T>(Func<DbSet<User>, IQueryable<T>> query)
  {
    using var context = ContextFactory.CreateDbContext();
    return await query(context.Users).ToListAsync();
  }
}
