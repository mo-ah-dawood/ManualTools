using System;
namespace ManualTools.Data.Entities
{
  public class Category
  {
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<Tool> Tools { get; set; } = new HashSet<Tool>();
  }
}

