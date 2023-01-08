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
public class ToolsController : ControllerBase
{

  private readonly ApplicationDbContext _context;
  private readonly IWebHostEnvironment _env;
  private readonly IMapper _mapper;
  public ToolsController(ApplicationDbContext context, IMapper mapper, IWebHostEnvironment env)
  {
    _context = context;
    _mapper = mapper;
    _env = env;
  }

  [HttpPost()]
  public async Task<ActionResult<CommonResponse<ToolViewModel>>> Create([FromForm] CreateToolInput model)
  {
    var path = SaveFile(model.Image!);
    try
    {

      var entity = _mapper.Map<Tool>(model);
      entity.Image = path;
      entity.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
      _context.Tools.Add(entity);
      await _context.SaveChangesAsync();
      return Ok(new CommonResponse<ToolViewModel>(_mapper.Map<ToolViewModel>(entity))); ;
    }
    catch (Exception e)
    {
      DeleteFile(path);
      return BadRequest(new CommonResponse<ToolViewModel>(e.Message));
    }
  }

  [HttpPost("{id}")]
  public async Task<ActionResult<CommonResponse<ToolViewModel>>> Update(int id, [FromForm] UpdateToolInput model)
  {
    var entity = await _context.Tools.FirstOrDefaultAsync((x) => x.Id == id);
    var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
    if (entity == null || entity.UserId != user)
    {
      return NotFound();
    }
    string image = entity.Image;
    if (model.Image != null)
      image = SaveFile(model.Image!);
    try
    {

      _mapper.Map(model, entity);
      entity.Image = image;
      await _context.SaveChangesAsync();
      return Ok(new CommonResponse<ToolViewModel>(_mapper.Map<ToolViewModel>(entity)));
    }
    catch (Exception e)
    {
      if (model.Image != null)
        DeleteFile(image);
      return BadRequest(new CommonResponse<ToolViewModel>(e.Message));
    }
  }


  [HttpGet()]
  public async Task<ActionResult<CommonResponse<List<ToolViewModel>>>> MyTools(int skip = 0, int? pageSize = default, string? q = default)
  {
    var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
    var query = _context.Tools.Where((w) => w.UserId == user);
    if (!string.IsNullOrWhiteSpace(q))
    {
      query = query.Where((w) => w.Name.Contains(q) || w.Description.Contains(q));
    }
    query = query.OrderByDescending((o) => o.Id);
    if (skip > 0)
    {
      query = query.Skip(skip);
    }
    if (pageSize.HasValue)
    {
      query = query.Take(pageSize.Value);
    }
    var list = await _mapper.ProjectTo<ToolViewModel>(query, null).ToListAsync();

    return Ok(new CommonResponse<List<ToolViewModel>>(list));
  }

  [HttpGet("{categoryId}")]
  [AllowAnonymous]
  public async Task<ActionResult<CommonResponse<List<ToolViewModel>>>> Tools(int categoryId, int skip = 0, int? pageSize = default, string? q = default)
  {
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    var query = _context.Tools
    .Where((w) => w.CategoryId == categoryId)
    .Where((w) => userId == null || w.UserId != userId);
    if (!string.IsNullOrWhiteSpace(q))
    {
      query = query.Where((w) => w.Name.Contains(q) || w.Description.Contains(q));
    }
    query = query.OrderByDescending((o) => o.Id);
    if (skip > 0)
    {
      query = query.Skip(skip);
    }
    if (pageSize.HasValue)
    {
      query = query.Take(pageSize.Value);
    }
    var list = await _mapper.ProjectTo<ToolViewModel>(query, null).ToListAsync();

    return Ok(new CommonResponse<List<ToolViewModel>>(list));
  }

  private string SaveFile(IFormFile file)
  {
    string path = Path.Combine(_env.WebRootPath, "Uploads/Tools");
    if (!Directory.Exists(path))
    {
      Directory.CreateDirectory(path);
    }
    var splits = Path.GetFileName(file.FileName).Split(".");
    var ext = splits.Length > 0 ? splits.Last() : "jpg";
    string fileName = $"{Guid.NewGuid()}.{ext}";
    path = Path.Combine(path, fileName);
    using FileStream stream = new(path, FileMode.Create);
    file.CopyTo(stream);
    return $"Uploads/Tools/{fileName}";
  }

  private static void DeleteFile(string file)
  {
    if (System.IO.File.Exists(file))
    {
      System.IO.File.Delete(file);
    }
  }
}

