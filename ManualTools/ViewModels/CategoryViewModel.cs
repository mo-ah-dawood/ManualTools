using AutoMapper;
using ManualTools.Common;

namespace ManualTools.Data.Entities
{
  public class CategoryViewModel : IMapFrom<Category>
  {
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int ToolsCount { get; set; }

    public void Configure(IMappingExpression mappingExpression)
    {

    }
  }
}

