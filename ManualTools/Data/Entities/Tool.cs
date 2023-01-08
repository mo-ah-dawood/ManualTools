using System;
namespace ManualTools.Data.Entities
{
  public class Tool
  {
    public Tool()
    {
    }

    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal DayPrice { get; set; }
    public User? User { get; set; }
    public string UserId { get; set; } = string.Empty;

    public Category? Category { get; set; }
    public int CategoryId { get; set; }
    public ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>();
  }
}

