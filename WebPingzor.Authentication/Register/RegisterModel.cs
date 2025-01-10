using System.ComponentModel.DataAnnotations;

namespace WebPingzor.Authentication.Register;

public class RegisterModel
{
  [Required]
  public string Name { get; set; } = string.Empty;
  [Required]
  public string Email { get; set; } = string.Empty;
  [Required]
  public string Password { get; set; } = string.Empty;
  [Required]
  public string ConfirmPassword { get; set; } = string.Empty;
}