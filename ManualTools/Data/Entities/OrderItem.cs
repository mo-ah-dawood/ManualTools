using System;
namespace ManualTools.Data.Entities
{
  public class OrderItem
  {
    public OrderItem()
    {
    }

    public Tool? Tool { get; set; }
    public int ToolId { get; set; }
    public int Days { get; set; }

    public Order? Order { get; set; }
    public int OrderId { get; set; }

    public decimal DayPrice { get; set; }
  }
}

