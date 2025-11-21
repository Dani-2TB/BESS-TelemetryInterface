using Microsoft.EntityFrameworkCore;
using DotnetAPI.Data;
using DotnetAPI.Models;
using DotnetAPI.Services;
using DotnetAPI.Models.Domain;
using DotNetEnv;
using DotNetEnv.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

Configure();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    SeedData.Initialize(services);
}

if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
    app.UseSwagger();
    app.UseSwaggerUI();
}

// CORS para permitir acceso desde internet
app.UseCors("AllowAll");

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapAuthEndpoints(); // Activar los endpoints JWT

app.MapStaticAssets();
app.MapRazorPages();

app.Run();

void Configure()
{
    // Database Configuration
    builder.Services.AddDbContext<YuzzContext>(opt =>
        opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

    // CORS Configuration para acceso desde internet
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
    });

    // Configuración de JWT - Lee desde User Secrets
    var jwtKey = builder.Configuration["JWT_TOKEN"] ?? throw new InvalidOperationException("JWT_TOKEN not found in configuration");
    var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "YuzzAPI";
    var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "YuzzClient";

    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    // Para Razor Pages: Lee token desde cookie SI existe
                    var token = context.Request.Cookies["jwtToken"];
                    if (!string.IsNullOrEmpty(token))
                    {
                        context.Token = token;
                    }
                    return Task.CompletedTask;
                }
            };
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtIssuer,
                ValidateAudience = true,
                ValidAudience = jwtAudience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromSeconds(30)
            };
        });

    // Servicios necesarios
    builder.Services.AddScoped<IPasswordHasher<AppUser>, PasswordHasher<AppUser>>();
    builder.Services.AddAuthorization();

    // Proteger páginas (requieren autenticación JWT)
    builder.Services.AddRazorPages(options =>
    {
        options.Conventions.AuthorizeFolder("/BessAdmin"); // Protege admin
        options.Conventions.AllowAnonymousToPage("/Auth/Index"); // Login sin auth
        options.Conventions.AllowAnonymousToPage("/Index"); // Home sin auth
    });

    builder.Services.AddHttpClient();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var configuration = new ConfigurationBuilder()
        .AddDotNetEnv(".env", LoadOptions.TraversePath())
        .Build();
}