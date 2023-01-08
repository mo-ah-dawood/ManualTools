using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using ManualTools.Common;

namespace ManualTools.Data.Entities
{
  public class UserViewModel : IMapFrom<User>
  {
    public string Id { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;

    public string BankAccount { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string ConfirmPassword { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;

    public void Configure(IMappingExpression mappingExpression)
    {

    }
  }
}

