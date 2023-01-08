using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using ManualTools.Common;

namespace ManualTools.Data.Entities
{
  public class RegisterInput : IMapTo<User>
  {
    [Required]
    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;
    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;
    [Required]
    [MaxLength(255)]
    [Display(Name = "Bank account")]
    public string BankAccount { get; set; } = string.Empty;
    [Required]
    [StringLength(20, MinimumLength = 6)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [Compare("Password")]
    [Display(Name = "Confirm password")]
    public string ConfirmPassword { get; set; } = string.Empty;

    public void Configure(IMappingExpression mappingExpression)
    {

    }
  }
}

