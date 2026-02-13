using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using GolekBackend.Data;
using GolekBackend.Models.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GolekBackend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
private readonly AppDbContext _context;
private readonly IConfiguration _config;

public AuthController(AppDbContext context, IConfiguration config)
{
_context = context;
_config = config;
}

[HttpPost("register")]
public async Task<IActionResult> Register([FromBody] User newUser)
{
// 1. Cek apakah username sudah dipakai
if (_context.Users.Any(u => u.Username == newUser.Username))
{
return BadRequest("Waduh King, username itu sudah ada yang punya!");
}

// 2. Simpan user baru ke database
_context.Users.Add(newUser);
await _context.SaveChangesAsync();

return Ok("User berhasil didaftarkan! Silakan lanjut Login, King.");
}

[HttpPost("login")]
public IActionResult Login([FromBody] User login)
{
// 1. Cek User di Database
var user = _context.Users.FirstOrDefault(u => u.Username == login.Username && u.Password == login.Password);

if (user == null) return Unauthorized("Username atau Password salah, King!");

// 2. Buat Token JWT
var jwtSettings = _config.GetSection("Jwt");
var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]!);

var tokenDescriptor = new SecurityTokenDescriptor
{
Subject = new ClaimsIdentity(new[] {
new Claim(ClaimTypes.Name, user.Username),
new Claim(ClaimTypes.Role, user.Role)
}),
Expires = DateTime.UtcNow.AddHours(3),
Issuer = jwtSettings["Issuer"],
Audience = jwtSettings["Audience"],
SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
};

var tokenHandler = new JwtSecurityTokenHandler();
var token = tokenHandler.CreateToken(tokenDescriptor);

return Ok(new { Token = tokenHandler.WriteToken(token) });
}
}