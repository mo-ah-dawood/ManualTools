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
public class CategoriesController : ControllerBase
{

  private readonly ApplicationDbContext _context;
  private readonly IMapper _mapper;
  public CategoriesController(ApplicationDbContext context, IMapper mapper)
  {
    _context = context;
    _mapper = mapper;
  }

  [HttpGet()]
  public async Task<ActionResult<CommonResponse<List<CategoryViewModel>>>> List()
  {

    var list = await _mapper.ProjectTo<CategoryViewModel>(_context.Categories.OrderByDescending((o) => o.Id), null).ToListAsync();

    return Ok(new CommonResponse<List<CategoryViewModel>>(list));
  }


}

