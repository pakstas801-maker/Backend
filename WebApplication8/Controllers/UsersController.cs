using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication8.Data;
using WebApplication8.Dto;

namespace WebApplication8.Controllers;


[ApiController]
[Route("api/users")]
[Authorize]
public class UsersController : ControllerBase
{

    
     private readonly AppDbContext _context;
     private readonly IConfiguration _config;

     public UsersController(AppDbContext context, IConfiguration config)
      {
         _context = context;
         _config = config;
      }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] Dto.LoginRequest request )
    {
        if (request == null || string.IsNullOrEmpty(request.Name) || string.IsNullOrEmpty(request.Password))
            return BadRequest("Invalid login request.");

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Name == request.Name);

        if (user == null) return Unauthorized("User not found.");

        if (!new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, request.Password).Equals(PasswordVerificationResult.Success))
            return Unauthorized("Invalid password.");



        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"])
        );

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: new SigningCredentials(
                key, SecurityAlgorithms.HmacSha256)
        );

        return Ok(new
        {
            token = new JwtSecurityTokenHandler().WriteToken(token)
        });
    }


    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _context.Users
            .Select(u => new UserDto
            {
                Id = u.Id,
                Name = u.Name
            })
            .ToListAsync();

        return Ok(users);
    }

    [AllowAnonymous]
    [HttpPost("Registration")]
    public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
    {
        var hasher = new PasswordHasher<User>();

        var user = new User
        {
            Name = dto.Name
        };

        user.PasswordHash = hasher.HashPassword(user, dto.Password);

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Created($"api/users/{user.Id}", new UserDto
        {
            Id = user.Id,
            Name = user.Name
        });
    }

    
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateUserDto dto)
    {
        var existing = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (existing == null)
            return NotFound();

        existing.Name = dto.Name;
        await _context.SaveChangesAsync();

        return Ok();
    }

   
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var existing = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (existing == null)
            return NotFound();

        _context.Users.Remove(existing);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    



}




