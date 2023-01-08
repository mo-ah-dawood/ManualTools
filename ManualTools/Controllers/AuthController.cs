using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using ManualTools.Common;
using ManualTools.Data;
using ManualTools.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ManualTools.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class AuthController : ControllerBase
{

  private readonly ApplicationDbContext _context;
  private readonly IConfiguration _config;
  private readonly IMapper _mapper;
  const int keySize = 64;
  const int iterations = 350000;
  readonly HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;
  public AuthController(ApplicationDbContext context, IMapper mapper, IConfiguration config)
  {
    _context = context;
    _mapper = mapper;
    _config = config;
  }

  [HttpPost(Name = "Register")]
  public async Task<ActionResult<CommonResponse<UserViewModel>>> Register([FromBody] RegisterInput model)
  {
    try
    {
      var oldUser = await _context.Users.FirstOrDefaultAsync((f) => f.Email == model.Email);
      if (oldUser != null)
      {
        return BadRequest(new CommonResponse<UserViewModel>("User with this email already exists"));
      }
      var user = _mapper.Map<User>(model);
      user.PasswordSalt = Encoding.UTF8.GetString(RandomNumberGenerator.GetBytes(keySize));
      user.Password = HashPassword(model.Password, user.PasswordSalt);
      _context.Users.Add(user);
      await _context.SaveChangesAsync();
      return Ok(new CommonResponse<UserViewModel>(MapUserAndGenerateToken(user))); ;
    }
    catch (Exception e)
    {
      return BadRequest(new CommonResponse<UserViewModel>(e.Message));
    }
  }

  [HttpPost(Name = "Login")]
  public async Task<ActionResult<CommonResponse<UserViewModel>>> Login([FromBody] LoginInput model)
  {
    try
    {
      var user = await _context.Users.FirstOrDefaultAsync((f) => f.Email == model.Email);
      if (user == null || user.Password != HashPassword(model.Password, user.PasswordSalt))
      {
        return BadRequest(new CommonResponse<UserViewModel>("Invalid credentials"));
      }
      return Ok(new CommonResponse<UserViewModel>(MapUserAndGenerateToken(user))); ;
    }
    catch (Exception e)
    {
      return BadRequest(new CommonResponse<UserViewModel>(e.Message));
    }
  }


  private UserViewModel MapUserAndGenerateToken(User user)
  {
    var result = _mapper.Map<UserViewModel>(user);

    var issuer = _config["Jwt:Issuer"];
    var audience = _config["Jwt:Audience"];
    var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]!);
    var tokenDescriptor = new SecurityTokenDescriptor
    {
      Subject = new ClaimsIdentity(new[]
        {
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim(JwtRegisteredClaimNames.Sub, user.Name),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, user.Id)
             }),
      Expires = DateTime.UtcNow.AddMinutes(5),
      Issuer = issuer,
      Audience = audience,
      SigningCredentials = new SigningCredentials
        (new SymmetricSecurityKey(key),
        SecurityAlgorithms.HmacSha512Signature)
    };
    var tokenHandler = new JwtSecurityTokenHandler();
    var token = tokenHandler.CreateToken(tokenDescriptor);
    var stringToken = tokenHandler.WriteToken(token);
    result.Token = stringToken;
    return result;
  }


  private string HashPassword(string password, string salt)
  {
    var _salt = Encoding.UTF8.GetBytes(salt);
    var hash = Rfc2898DeriveBytes.Pbkdf2(
        Encoding.UTF8.GetBytes(password),
        _salt,
        iterations,
        hashAlgorithm,
        keySize);
    return Convert.ToHexString(hash);
  }


}

