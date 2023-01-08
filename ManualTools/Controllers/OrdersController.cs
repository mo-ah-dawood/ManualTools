using System.Security.Claims;
using AutoMapper;
using ManualTools.Common;
using ManualTools.Data;
using ManualTools.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ManualTools.Controllers;

[ApiController]
[Route("[controller]/[action]")]
[Authorize]
public class OrdersController : ControllerBase
{

  private readonly ApplicationDbContext _context;
  private readonly IMapper _mapper;
  public OrdersController(ApplicationDbContext context, IMapper mapper)
  {
    _context = context;
    _mapper = mapper;
  }

  [HttpPost()]
  public async Task<ActionResult<CommonResponse<OrderViewModel>>> Create([FromBody] CreateOrderInput model)
  {
    try
    {
      string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
      var items = model.Items.GroupBy((s) => s.ToolId)
      .Select((x) => new
      {
        x.Key,
        Days = x.Sum((a) => a.Days)
      })
      .Distinct().ToList();
      var ids = items.Select((x) => x.Key).ToList();
      var tools = _context.Tools.Where((a) => ids.Contains(a.Id)).ToList();
      if (!items.Any((i) => tools.Any((t) => t.Id == i.Key)))
      {
        return NotFound();
      }
      if (tools.Any((a) => a.UserId == userId))
      {
        return BadRequest(new CommonResponse<OrderViewModel>("Sorry! you can't order your own tool"));
      }

      using var transaction = _context.Database.BeginTransaction();
      var order = new Order() { ClientId = userId! };
      _context.Orders.Add(order);
      await _context.SaveChangesAsync();
      foreach (var item in items)
      {
        var tool = tools.First((i) => i.Id == item.Key);
        _context.OrderItems.Add(new OrderItem()
        {
          DayPrice = tool.DayPrice,
          Days = item.Days,
          ToolId = tool.Id,
          OrderId = order.Id,
        });
      }
      await _context.SaveChangesAsync();
      transaction.Commit();
      return Ok(new CommonResponse<OrderViewModel>(_mapper.Map<OrderViewModel>(order)));

    }
    catch (Exception e)
    {
      return BadRequest(new CommonResponse<ToolViewModel>(e.Message));
    }
  }

  [HttpGet()]
  public async Task<ActionResult<CommonResponse<List<OrderViewModel>>>> MyOrders(int skip = 0, int? pageSize = default)
  {
    var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
    var query = _context.Orders.Where((w) => w.ClientId == user);
    query = query.OrderByDescending((o) => o.Id);
    if (skip > 0)
    {
      query = query.Skip(skip);
    }
    if (pageSize.HasValue)
    {
      query = query.Take(pageSize.Value);
    }
    var list = await _mapper.ProjectTo<OrderViewModel>(query, null).ToListAsync();

    return Ok(new CommonResponse<List<OrderViewModel>>(list));
  }


  [HttpGet()]
  public async Task<ActionResult<CommonResponse<List<OrderViewModel>>>> MyOrdered(int skip = 0, int? pageSize = default)
  {
    var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
    var query = _context.Orders.Where((w) => w.Items.Any((x) => x.Tool!.UserId == user));
    query = query.OrderByDescending((o) => o.Id);
    if (skip > 0)
    {
      query = query.Skip(skip);
    }
    if (pageSize.HasValue)
    {
      query = query.Take(pageSize.Value);
    }
    var list = await _mapper.ProjectTo<OrderViewModel>(query, new Dictionary<string, object?>(){
      {"userId",user}
    }).ToListAsync();

    return Ok(new CommonResponse<List<OrderViewModel>>(list));
  }


}

