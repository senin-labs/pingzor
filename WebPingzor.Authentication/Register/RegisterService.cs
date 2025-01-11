using WebPingzor.Core;
using WebPingzor.Data;
using WebPingzor.Data.Models;

namespace WebPingzor.Authentication.Register
{
  public class RegisterService
  {
    private readonly PingzorDbProvider _dbProvider;
    private readonly IPasswordHashingService _hashingService;

    public RegisterService(PingzorDbProvider dbProvider, IPasswordHashingService passwordHashingService)
    {
      this._dbProvider = dbProvider;
      this._hashingService = passwordHashingService;
    }

    public async Task<User> Register(string name, string email, string password)
    {
      using var context = _dbProvider.Create();

      var salt = _hashingService.GenerateSalt();
      var hashedPassword = _hashingService.HashPassword(password, salt);

      var newUser = new User
      {
        Name = name,
        Email = email,
        PasswordSalt = salt,
        HashedPassword = hashedPassword
      };

      context.Users.Add(newUser);

      await context.SaveChangesAsync();

      return newUser;
    }
  }
}