using System;
namespace ManualTools.Data.Entities
{
  public class Order
  {
    public Order()
    {
    }
    public int Id { get; set; }
    public User? Client { get; set; }
    public string ClientId { get; set; } = string.Empty;
    public ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>();
  }
}

