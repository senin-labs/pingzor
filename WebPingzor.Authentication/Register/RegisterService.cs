using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using WebPingzor.Core;
using WebPingzor.Data;
using WebPingzor.Data.Models;

namespace WebPingzor.Authentication.Register;
public class RegisterService(
  PingzorDbProvider _dbProvider,
  IPasswordHashingService _hashingService
)
{
  public async Task<User> Register(string name, string email, string password)
  {
    using var context = _dbProvider.Create();

    var userExists = await context.Users.AnyAsync(u => u.Email.ToLower() == email.ToLower());
    if (userExists)
    {
      throw new ValidationException("User already exists.");
    }

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