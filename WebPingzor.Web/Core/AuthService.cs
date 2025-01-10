using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using WebPingzor.Core;
using WebPingzor.Data;
using Microsoft.EntityFrameworkCore;

namespace WebPingzor.Web.Core
{
  internal class AuthService(
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
      throw new NotImplementedException();
    }

    public Task<bool> IsAuthenticated()
    {
      throw new NotImplementedException();
    }

    public async Task Login(string email, string password)
    {
      var context = _contextAccessor.HttpContext ?? throw new Exception("HttpContext is null");

      using var db = _dbProvider.Create();
      var user = await db.Users.Select(u => new
      {
        Id = u.Id,
        Name = u.Name,
        Email = u.Email,
        HashedPassword = u.HashedPassword,
        PasswordSalt = u.PasswordSalt
      }).FirstOrDefaultAsync(u => u.Email == email);

      if (user == null)
      {
        throw new Exception("User or password is incorrect");
      }

      var isValid = _hashingService.VerifyPassword(password, user.PasswordSalt, user.HashedPassword);
      if (!isValid)
      {
        throw new Exception("User or password is incorrect");
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

    public Task Logout()
    {
      throw new NotImplementedException();
    }
  }
}