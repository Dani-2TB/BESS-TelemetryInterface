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

        // ---------- Endpoints ----------

        // Registrar usuario
        routes.MapPost("/auth/register", async (
            RegisterRequest req,
            YuzzContext db) =>
        {
            var userName = req.UserName.Trim();
            var email = req.Email.Trim().ToLowerInvariant();

            var queriesResult = db.Users.Where(u => u.UserName == userName || u.Email == email);

            if (queriesResult.Count() > 0)
                return Results.BadRequest(new { message = "Error 4" });

            var user = new AppUser { UserName = userName, Email = email };
            user.PasswordHash = req.Password.HashPassword();

            db.Users.Add(user);
            await db.SaveChangesAsync();

            return Results.Created($"/users/{user.Id}",
                new { user.Id, user.UserName, user.Email });
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

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return Results.Ok(new AuthResponse
            {
                AccessToken = jwt,
                ExpiresAtUtc = expires
            });
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
