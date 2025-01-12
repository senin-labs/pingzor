using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using WebPingzor.Core;
using WebPingzor.Data;

namespace WebPingzor.Web.Core
{
  internal sealed class AuthService(
    IHttpContextAccessor httpContextAccessor,
    IPasswordHashingService hashingService,
    PingzorDbProvider dbProvider
  ) : IAuthService
  {
    private readonly IHttpContextAccessor _contextAccessor = httpContextAccessor;
    private readonly IPasswordHashingService _hashingService = hashingService;
    private readonly PingzorDbProvider _dbProvider = dbProvider;

    public Task<string> GetUsername()
    {
      var context = _contextAccessor.HttpContext ?? throw new Exception("HttpContext is null");
      return Task.FromResult(context.User.Identity?.Name ?? string.Empty);
    }

    public Task<int?> GetUserId()
    {
      var context = _contextAccessor.HttpContext ?? throw new Exception("HttpContext is null");
      var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      if (int.TryParse(userId, out var id))
      {
        return Task.FromResult(id as int?);
      }

      return Task.FromResult(null as int?);
    }

    public Task<bool> IsAuthenticated()
    {
      var context = _contextAccessor.HttpContext ?? throw new Exception("HttpContext is null");
      return Task.FromResult(context.User.Identity?.IsAuthenticated ?? false);
    }

    public async Task Login(string email, string password)
    {
      var context = _contextAccessor.HttpContext ?? throw new Exception("HttpContext is null");

      using var db = _dbProvider.Create();
      var user = await db.Users.Select(u => new
      {
        u.Id,
        u.Name,
        u.Email,
        u.HashedPassword,
        u.PasswordSalt
      }).FirstOrDefaultAsync(u => u.Email == email);

      if (user == null)
      {
        throw new ValidationException("User or password is incorrect");
      }

      var isValid = _hashingService.VerifyPassword(password, user.PasswordSalt, user.HashedPassword);
      if (!isValid)
      {
        throw new ValidationException("User or password is incorrect");
      }

      var claims = new List<Claim>
      {
        new(ClaimTypes.Name, user.Name),
        new(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new(ClaimTypes.Email, email)
      };

      var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
      var principal = new ClaimsPrincipal(identity);

      var authProps = new AuthenticationProperties
      {
        IsPersistent = true,
        ExpiresUtc = DateTime.UtcNow.AddHours(1)
      };

      await context.SignInAsync(
        CookieAuthenticationDefaults.AuthenticationScheme,
        principal,
        authProps
      );
    }

    public async Task Logout()
    {
      var context = _contextAccessor.HttpContext ?? throw new Exception("HttpContext is null");
      await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }
  }
}