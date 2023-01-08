using AutoMapper;
using ManualTools.Common;

namespace ManualTools.Data.Entities
{
  public class OrderViewModel : IMapFrom<Order, OrderViewModel>
  {

    public ICollection<OrderItemViewModel> Items { get; set; } = new HashSet<OrderItemViewModel>();
    public decimal Total => Items.Sum((x) => x.Total);

    public void Configure(IMappingExpression<Order, OrderViewModel> mappingExpression)
    {
      string? userId = null;
      mappingExpression.ForMember((x) => x.Items, (o) => o.MapFrom((x) => x.Items.Where((x) => userId == null || x.Tool!.UserId == userId)));
    }

  }



  public class OrderItemViewModel : IMapFrom<OrderItem>
  {

    public int Days { get; set; }
    public decimal DayPrice { get; set; }
    public decimal Total => Days * DayPrice;

    public void Configure(IMappingExpression mappingExpression)
    {

    }

  }
}

