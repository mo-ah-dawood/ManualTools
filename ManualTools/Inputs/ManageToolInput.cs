using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using ManualTools.Common;

namespace ManualTools.Data.Entities
{
  public class CreateToolInput : IMapTo<Tool>
  {
    [Required]
    [MinLength(10)]
    public string Name { get; set; } = string.Empty;
    [Required]
    [MinLength(10)]
    public string Description { get; set; } = string.Empty;
    [Required]
    [Range(1.0, 1000000)]
    public decimal DayPrice { get; set; }
    [Required]
    public IFormFile? Image { get; set; }
    [Required]
    public int CategoryId { get; set; }

    public void Configure(IMappingExpression mappingExpression)
    {
      mappingExpression.ForMember("Image", (opt) => opt.Ignore());
    }
  }


  public class UpdateToolInput : IMapTo<Tool>
  {
    [Required]
    [MinLength(10)]
    public string Name { get; set; } = string.Empty;
    [Required]
    [MinLength(10)]
    public string Description { get; set; } = string.Empty;
    [Required]
    [Range(1.0, 1000000)]
    public decimal DayPrice { get; set; }
    [Required]
    public int CategoryId { get; set; }
    public IFormFile? Image { get; set; }

    public void Configure(IMappingExpression mappingExpression)
    {
      mappingExpression.ForMember("Image", (opt) => opt.Ignore());
    }
  }

}

