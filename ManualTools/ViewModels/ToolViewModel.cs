using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using ManualTools.Common;

namespace ManualTools.Data.Entities
{
  public class ToolViewModel : IMapFrom<Tool>
  {
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal DayPrice { get; set; }
    public string? UserId { get; set; }
    public int CategoryId { get; set; }

    public void Configure(IMappingExpression mappingExpression)
    {

    }
  }
}

