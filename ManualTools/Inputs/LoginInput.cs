using System;
using System.ComponentModel.DataAnnotations;

namespace ManualTools.Data.Entities
{
  public class LoginInput
  {
    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(20, MinimumLength = 6)]
    public string Password { get; set; } = string.Empty;

  }
}

