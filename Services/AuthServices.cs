using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DotnetAPI.Models.Domain;
using DotnetAPI.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using DotnetAPI.Extensions;

namespace DotnetAPI.Services;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder routes)
    {
        var builder = routes as WebApplication; // Para acceder a configuración
        if (builder is null) throw new InvalidOperationException("Debe ser WebApplication");

        // Obtener configuración JWT desde appsettings
        var jwtSection = builder.Configuration.GetSection("Jwt");
        var issuer = jwtSection["Issuer"]!;
        var audience = jwtSection["Audience"]!;
        var key = builder.Configuration["JWT_TOKEN"]!;
        var expiresMinutes = int.Parse(jwtSection["ExpiresMinutes"] ?? "10");
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

        // ---------- Endpoints ---------
        // Registrar usuario
        routes.MapPost("/auth/register", async (RegisterRequest req, YuzzContext db, IPasswordHasher<AppUser> hasher) =>
        {
            if (string.IsNullOrWhiteSpace(req.UserName) || string.IsNullOrWhiteSpace(req.Email) || string.IsNullOrWhiteSpace(req.Password))
            {
                return Results.BadRequest(new { message = "Todos los campos son obligatorios." });
            }

            var exists = await db.Users.AnyAsync(u => u.UserName == req.UserName || u.Email == req.Email);
            if (exists)
            {
                return Results.Conflict(new { message = "El usuario o email ya existe." });
            }

            var user = new AppUser
            {
                UserName = req.UserName,
                Email = req.Email,
                PasswordHash = hasher.HashPassword(null, req.Password) // <-- Cambia esto
            };
            db.Users.Add(user);
            await db.SaveChangesAsync();

            return Results.Created($"/users/{user.Id}", new { message = "Usuario registrado correctamente." });
        });

        // Login
        routes.MapPost("/auth/login", async (
            LoginRequest req,
            YuzzContext db,
            IPasswordHasher<AppUser> hasher) =>
        {
            var query = req.UserNameOrEmail.Trim();
            var user = await db.Users
                .FirstOrDefaultAsync(u => u.UserName == query || u.Email == query.ToLower());

            if (user is null) return Results.Unauthorized();

            var verify = hasher.VerifyHashedPassword(user, user.PasswordHash, req.Password);
            if (verify == PasswordVerificationResult.Failed)
                return Results.Unauthorized();

            var now = DateTime.UtcNow;
            var expires = now.AddMinutes(expiresMinutes);

            var claims = new List<Claim>
            {
                 new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                 new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                 new Claim(JwtRegisteredClaimNames.Email, user.Email),
                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                notBefore: now,
                expires: expires,
                signingCredentials: creds
            );

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return Results.Ok(new { token = jwtToken });
        });

        // Usuario autenticado
        routes.MapGet("/auth/me", async (ClaimsPrincipal user, YuzzContext db) =>
        {
            var userId = user.FindFirstValue(JwtRegisteredClaimNames.Sub);
            if (userId is null) return Results.Unauthorized();

            var uid = Guid.Parse(userId);
            var entity = await db.Users
                .Where(u => u.Id == uid)
                .Select(u => new { u.Id, u.UserName, u.Email })
                .FirstOrDefaultAsync();

            return entity is null ? Results.Unauthorized() : Results.Ok(entity);
        })
        .RequireAuthorization();

        return routes;
    }
}

// Model for registering user
public class RegisterRequest
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}

// Modelo para el login
public class LoginRequest
{
    public string UserNameOrEmail { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
