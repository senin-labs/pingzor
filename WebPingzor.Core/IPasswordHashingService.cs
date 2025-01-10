namespace WebPingzor.Core
{
  public interface IPasswordHashingService
  {
    string HashPassword(string password, string salt);
    string GenerateSalt();
    bool VerifyPassword(string password, string salt, string hashedPassword);
  }
}