using ManualTools.Data.Entities;
using Microsoft.EntityFrameworkCore;
namespace ManualTools.Data;

public class ApplicationDbContext : DbContext
{
  public ApplicationDbContext(DbContextOptions options) : base(options)
  {
  }
  public DbSet<Category> Categories => Set<Category>();
  public DbSet<User> Users => Set<User>();
  public DbSet<Tool> Tools => Set<Tool>();
  public DbSet<Order> Orders => Set<Order>();
  public DbSet<OrderItem> OrderItems => Set<OrderItem>();

  protected override void OnModelCreating(ModelBuilder builder)
  {
    builder.Entity<OrderItem>().HasKey((x) => new { x.OrderId, x.ToolId });
    builder.Entity<Order>().HasMany((x) => x.Items).WithOne((x) => x.Order).OnDelete(DeleteBehavior.Cascade);
    builder.Entity<Tool>().HasMany((x) => x.Items).WithOne((x) => x.Tool).OnDelete(DeleteBehavior.Restrict);
    builder.Entity<Category>().HasData(new Category()
    {
      Id = 1,
      Name = "Manual Tools"
    }, new Category()
    {
      Id = 2,
      Name = "Electronic Tools"
    }, new Category()
    {
      Id = 3,
      Name = "Devices"
    }, new Category()
    {
      Id = 4,
      Name = "Drilling equipment"
    }, new Category()
    {
      Id = 5,
      Name = "Construction equipment"
    }, new Category()
    {
      Id = 6,
      Name = "Agriculture equipment"
    });
  }
}

