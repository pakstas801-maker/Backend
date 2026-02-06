using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using WebApplication8.Data;
using Microsoft.AspNetCore.Mvc;
using WebApplication8.Dto;
using System.Collections.Immutable;
namespace WebApplication8.ChangePasswordDto;


[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _context;

    public UsersController(AppDbContext context)
    {
        _context = context;
    }





    static List<User> users = new()
    {
        

    new User { Id = 1, Name = "Alice", PasswordHash = "3442"},
        new User { Id = 2, Name = "Bob", PasswordHash = "9595" },
        new User { Id = 3, Name = "Charlie", PasswordHash = "1234" },
        new User { Id = 4, Name = "David", PasswordHash = "4414" },
    };

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _context.Users
        .Select (u => new UserDto
        {
            Id = u.Id,
            Name = u.Name
        }).ToListAsync();
        return Ok(users);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
    {
        var hasher = new PasswordHasher<User>();

        var user = new User
        {
            Name = dto.Name,
            
        };

        user.PasswordHash = hasher.HashPassword(user, dto.Password);

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return Created($"api/users/{user.Id}", new UserDto
        {
            Id = user.Id,
            Name = user.Name
        });
    }

    [HttpPut ("{id}")]
    public async Task<IActionResult> Update(int id, UpdateUserDto dto)
    {
        var existing = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

        if (existing == null)
        
            return NotFound();

        existing.Name = dto.Name;
        await _context.SaveChangesAsync();
        return Ok(existing);
            
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete (int id)
    {
        var existing = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (existing == null)
        {
            return NotFound();
        }   
        _context.Users.Remove(existing);
        _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Name == dto.Name);
        if (user == null)
            return Unauthorized();

        var hasher = new PasswordHasher<User>();
        var result = hasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);

        if (result == PasswordVerificationResult.Failed)
            return Unauthorized();

        return Ok(new UserDto
        {
            Id = user.Id,
            Name = user.Name
        });

            
    }


}

 