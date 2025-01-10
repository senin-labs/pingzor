using System.ComponentModel.DataAnnotations;

namespace WebPingzor.Authentication.Login;

public class LoginModel
{
  [Required]
  public string Username { get; set; } = string.Empty;
  [Required]
  public string Password { get; set; } = string.Empty;
  public bool RememberMe { get; set; } = true;
}