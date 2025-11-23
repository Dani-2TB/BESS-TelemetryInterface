using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DotnetAPI.Models.Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace DotnetAPI.Services;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/auth"); // Grouping for better API structure

        group.MapPost("/register", Register)
             .RequireAuthorization(p => p.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme));
        group.MapPost("/login", Login);
        
        // Explicitly require JWT Scheme for this endpoint to avoid confusion with Cookies
        group.MapGet("/me", GetCurrentUser)
             .RequireAuthorization(p => p.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme));

        return routes;
    }

    private static async Task<IResult> Register(
        RegisterRequest req, 
        UserManager<AppUser> userManager)
    {
        if (string.IsNullOrWhiteSpace(req.UserName) || string.IsNullOrWhiteSpace(req.Email))
            return Results.BadRequest(new { message = "Invalid input data." });

        var user = new AppUser
        {
            UserName = req.UserName,
            Email = req.Email,
        };

        // UserManager handles hashing, salting, and validation automatically
        var result = await userManager.CreateAsync(user, req.Password);

        if (!result.Succeeded)
        {
            return Results.BadRequest(result.Errors);
        }

        return Results.Created($"/users/{user.Id}", new { message = "User registered successfully." });
    }

    private static async Task<IResult> Login(
        LoginRequest req, 
        UserManager<AppUser> userManager, 
        IConfiguration config)
    {
        // Find User (Supports UserName or Email)
        var user = await userManager.FindByNameAsync(req.UserNameOrEmail);
        if (user == null && req.UserNameOrEmail.Contains("@"))
        {
            user = await userManager.FindByEmailAsync(req.UserNameOrEmail);
        }

        if (user is null) return Results.Unauthorized();

        // Check Password using UserManager
        // This implicitly checks the 'Lockout' status if enabled in options
        var isValid = await userManager.CheckPasswordAsync(user, req.Password);
        
        if (!isValid) 
        {
            // Increment access failed count here using userManager.AccessFailedAsync(user)
            return Results.Unauthorized();
        }

        // Generate Token
        var token = GenerateJwt(user, config);
        return Results.Ok(new { token });
    }

    private static async Task<IResult> GetCurrentUser(
        ClaimsPrincipal principal, 
        UserManager<AppUser> userManager)
    {
        // Extract ID from the 'sub' claim standard
        var userId = principal.FindFirstValue(JwtRegisteredClaimNames.Sub);
        if (userId is null) return Results.Unauthorized();

        var user = await userManager.FindByIdAsync(userId);
        return user is null 
            ? Results.Unauthorized() 
            : Results.Ok(new { user.Id, user.UserName, user.Email });
    }

    private static string GenerateJwt(AppUser user, IConfiguration config)
    {
        var jwtSettings = config.GetSection("Jwt");
        var key = Encoding.UTF8.GetBytes(config["JWT_TOKEN"] 
            ?? throw new InvalidOperationException("JWT Key missing"));

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, user.UserName!),
            new(JwtRegisteredClaimNames.Email, user.Email!),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            // Security Stamp is critical for invalidating tokens if password changes
            new("AspNet.Identity.SecurityStamp", user.SecurityStamp ?? "")
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpiresMinutes"] ?? "60")),
            Issuer = jwtSettings["Issuer"],
            Audience = jwtSettings["Audience"],
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key), 
                SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}

// Request Models
public record RegisterRequest(string UserName, string Email, string Password);
public record LoginRequest(string UserNameOrEmail, string Password);