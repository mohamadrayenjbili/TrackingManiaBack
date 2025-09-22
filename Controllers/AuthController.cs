using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using TrackMania.Data;
using TrackMania.Models;

namespace TrackMania.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;

    public AuthController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("signup")]
    public async Task<IActionResult> SignUp([FromBody] SignupDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Uniqueness checks
        if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
            return Conflict("Email déjà utilisé");
        if (await _context.Users.AnyAsync(u => u.Username == dto.Username))
            return Conflict("Nom d'utilisateur déjà utilisé");

        User newUser;
        if (string.Equals(dto.Role, "Admin", StringComparison.OrdinalIgnoreCase))
        {
            newUser = new Admin
            {
                Username = dto.Username,
                Email = dto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Photo = dto.Photo ?? string.Empty,
                DepartementId = dto.DepartementId ?? 0
            };
        }
        else if (string.Equals(dto.Role, "Ingenieur", StringComparison.OrdinalIgnoreCase))
        {
            newUser = new Ingenieur
            {
                Username = dto.Username,
                Email = dto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Photo = dto.Photo ?? string.Empty,
                DepartementId = dto.DepartementId ?? 0
            };
        }
        else
        {
            return BadRequest("Role invalide. Utilisez 'Admin' ou 'Ingenieur'.");
        }

        // Validate departement for roles that require it
        if (newUser is Ingenieur ing && (ing.DepartementId <= 0 || !await _context.Departements.AnyAsync(d => d.Id == ing.DepartementId)))
        {
            return BadRequest("Le département spécifié n'existe pas");
        }

        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();
        var createdRole = newUser switch { Admin => "admin", Ingenieur => "ingenieur", _ => "user" };
        return Created($"/api/users/{newUser.Id}", new { id = newUser.Id, username = newUser.Username, email = newUser.Email, role = createdRole });
    }

    [HttpPost("signin")]
    public async Task<IActionResult> SignIn([FromBody] SigninDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Find by email or username
        User? user = null;
        if (!string.IsNullOrWhiteSpace(dto.Email))
        {
            user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
        }
        else if (!string.IsNullOrWhiteSpace(dto.Username))
        {
            user = await _context.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);
        }

        if (user == null)
            return Unauthorized("Identifiants invalides");

        // Verify password (BCrypt)
        if (string.IsNullOrWhiteSpace(dto.Password) || !BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
            return Unauthorized("Identifiants invalides");

        var role = user switch
        {
            Admin => "admin",
            Ingenieur => "ingenieur",
            _ => "user"
        };

        return Ok(new { id = user.Id, username = user.Username, email = user.Email, role });
    }
}

public class SignupDto
{
    [Required]
    [MinLength(3)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    public string Password { get; set; } = string.Empty;

    [Required]
    public string Role { get; set; } = string.Empty; // Admin | Ingenieur

    public int? DepartementId { get; set; }

    public string? Photo { get; set; }
}

public class SigninDto
{
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
}



