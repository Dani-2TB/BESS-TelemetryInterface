using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DotnetAPI.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// --- Configuración de servicios ---
// DbContext
builder.Services.AddDbContext<YuzzContext>(opt =>
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Hasher de contraseñas
builder.Services.AddScoped<IPasswordHasher<AppUser>, PasswordHasher<AppUser>>();

// JWT
var jwtSection = builder.Configuration.GetSection("Jwt");
var issuer = jwtSection["Issuer"]!;
var audience = jwtSection["Audience"]!;
var key = jwtSection["Key"]!;
var expiresMinutes = int.Parse(jwtSection["ExpiresMinutes"] ?? "60");
var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = issuer,
            ValidateAudience = true,
            ValidAudience = audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = signingKey,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromSeconds(30)
        };
    });

builder.Services.AddAuthorization();

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Razor Pages y Controllers solo si los necesitas
builder.Services.AddRazorPages();
builder.Services.AddControllers();

var app = builder.Build();

// --- Pipeline HTTP ---
// Solo en Development
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();  
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Redirección HTTPS
app.UseHttpsRedirection();

// Middleware de autenticación/autorización
app.UseAuthentication();
app.UseAuthorization();

// Mapear Razor Pages y Controllers (opcional)
app.MapControllers();
app.MapRazorPages();

// Mapear tus módulos
app.AddModulesEndpoints();
app.AddConfigBessEndpoints();

// --- Healthcheck y endpoints de prueba ---
app.MapGet("/", () => Results.Ok(new { ok = true, name = "AuthApi" }));
app.MapGet("/ping", () => "API funcionando!");

// --- Endpoints de autenticación ---
// Registrar usuario
app.MapPost("/auth/register", async (
    RegisterRequest req,
    YuzzContext db,
    IPasswordHasher<AppUser> hasher) =>
{
    var userName = req.UserName.Trim();
    var email = req.Email.Trim().ToLowerInvariant();

    if (await db.Users.AnyAsync(u => u.UserName == userName))
        return Results.BadRequest(new { message = "UserName ya existe" });

    if (await db.Users.AnyAsync(u => u.Email == email))
        return Results.BadRequest(new { message = "Email ya registrado" });

    var user = new AppUser { UserName = userName, Email = email };
    user.PasswordHash = hasher.HashPassword(user, req.Password);

    db.Users.Add(user);
    await db.SaveChangesAsync();

    return Results.Created($"/users/{user.Id}", new { user.Id, user.UserName, user.Email });
});

// Login
app.MapPost("/auth/login", async (
    LoginRequest req,
    YuzzContext db,
    IPasswordHasher<AppUser> hasher) =>
{
    var query = req.UserNameOrEmail.Trim();
    var user = await db.Users
        .Where(u => u.UserName == query || u.Email == query.ToLower())
        .FirstOrDefaultAsync();

    if (user is null)
        return Results.Unauthorized();

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

// Endpoint protegido
app.MapGet("/auth/me", async (ClaimsPrincipal user, YuzzContext db) =>
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

// --- Migraciones automáticas ---
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<YuzzContext>();
    await db.Database.MigrateAsync();
}

app.Run();
