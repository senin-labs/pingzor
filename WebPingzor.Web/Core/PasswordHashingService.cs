using WebPingzor.Core;
using System.Security.Cryptography;
using System.Text;

namespace WebPingzor.Web.Core;
internal sealed class PasswordHashingService : IPasswordHashingService
{
  public string HashPassword(string password, string salt)
  {
    using var sha256 = SHA256.Create();
    var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password + salt));
    return BitConverter.ToString(hashedBytes).Replace("-", "");
  }

  public string GenerateSalt()
  {
    var random = new Random();
    var salt = new byte[16];
    random.NextBytes(salt);
    return BitConverter.ToString(salt).Replace("-", "");
  }

  public bool VerifyPassword(string password, string salt, string hashedPassword)
  {
    return HashPassword(password, salt) == hashedPassword;
  }
}