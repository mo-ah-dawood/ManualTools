using System;
using System.ComponentModel.DataAnnotations;

namespace ManualTools.Data.Entities
{

  public class CreateOrderInput
  {
    [Required]
    [MinLength(1)]
    public IList<CreateOrderItemInput> Items { get; set; } = new List<CreateOrderItemInput>();

  }
  public class CreateOrderItemInput
  {
    [Required]
    public int ToolId { get; set; }

    [Required]
    [Range(1, 360)]
    public int Days { get; set; }
  }
}

