using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using DotnetAPI.Data;
using DotnetAPI.Models.Domain;
using DotnetAPI.Models;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Load env vars immediately to ensure DB connection string is available
Env.Load(".env");
builder.Configuration.AddEnvironmentVariables();

ConfigureServices(builder);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try 
    {
        // Auto-migrate on startup to ensure DB schema matches code in the device
        var context = services.GetRequiredService<YuzzContext>();
        context.Database.Migrate();
        await SeedData.InitializeAsync(services);
    }
    catch (Exception ex)
    {
        services.GetRequiredService<ILogger<Program>>().LogError(ex, "DB Seeding/Migration failed.");
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // Enforce strict HTTPS in production/staging
    app.UseHsts(); 
}

// Enable Swagger in all environments for local device debugging
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

app.Run();

void ConfigureServices(WebApplicationBuilder builder)
{
    builder.Services.AddDbContext<YuzzContext>(opt =>
        opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

    // Identity handles Cookie auth by default for Razor Pages
    builder.Services.AddIdentity<AppUser, IdentityRole<Guid>>(options => 
    {
        options.Password.RequireDigit = true; 
        options.Password.RequiredLength = 8; 
        options.SignIn.RequireConfirmedAccount = false;
    })
    .AddEntityFrameworkStores<YuzzContext>()
    .AddDefaultTokenProviders();

    // Security hardening for session cookies
    builder.Services.ConfigureApplicationCookie(options =>
    {
        options.LoginPath = "/Auth/Index";
        options.AccessDeniedPath = "/Error";
        options.Cookie.HttpOnly = true; // Prevent XSS stealing
        options.Cookie.SameSite = SameSiteMode.Strict; // CSRF mitigation
    });

    // JWT setup as a specific scheme. API Controllers must explicitly ask for this scheme
    // using [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    var jwtKey = builder.Configuration["JWT_TOKEN"] ?? throw new InvalidOperationException("JWT_TOKEN missing");

    builder.Services.AddAuthentication()
        .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? "YuzzAPI",
                ValidateAudience = true,
                ValidAudience = builder.Configuration["Jwt:Audience"] ?? "YuzzClient",
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero // Strict expiration
            };
        });

    builder.Services.AddScoped<IPasswordHasher<AppUser>, PasswordHasher<AppUser>>();
    
    builder.Services.AddRazorPages(options =>
    {
        options.Conventions.AuthorizeFolder("/BessAdmin");
    });
    
    // Required for API endpoints
    builder.Services.AddControllers(); 
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddHttpClient();

    // Configure Swagger to allow Bearer token testing in UI
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Yuzz BESS API", Version = "v1" });
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "bearer"
        });
        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                },
                Array.Empty<string>()
            }
        });
    });
}