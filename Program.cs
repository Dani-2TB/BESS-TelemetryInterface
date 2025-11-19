using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using DotnetAPI.Data;
using DotnetAPI.Models.Domain;
using DotNetEnv;
using DotNetEnv.Configuration;
using DotnetAPI.Models;

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
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

//app.MapAuthEndpoints();

app.MapStaticAssets();
app.MapRazorPages();

app.Run();


void Configure()
{
    // Database Configuration
    builder.Services.AddDbContext<YuzzContext>(opt =>
        opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

    // Identity Configuration
    builder.Services.AddIdentity<AppUser, IdentityRole<Guid>>(options => 
    {
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequiredLength = 4;
    })
    .AddEntityFrameworkStores<YuzzContext>()
    .AddDefaultTokenProviders();

    // Cookie Configuration
    builder.Services.ConfigureApplicationCookie(options =>
    {
        options.LoginPath = "/Auth/Index";
        options.AccessDeniedPath = "/Error";
    });

    builder.Services.AddAuthorization();

    builder.Services.AddRazorPages(options =>
    {
        options.Conventions.AuthorizeFolder("/BessAdmin");
    });
    builder.Services.AddHttpClient(); 

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var configuration = new ConfigurationBuilder()
        .AddDotNetEnv(".env", LoadOptions.TraversePath())
        .Build();
}