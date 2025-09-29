using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DotnetAPI.Data;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DotnetAPI.Services;

var builder = WebApplication.CreateBuilder(args);

Configure();

var app = builder.Build();

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

app.UseAuthorization();

app.UseAuthentication();

//app.MapAuthEndpoints();

app.MapStaticAssets();
app.MapRazorPages();

app.Run();


void Configure()
{
    // Database Configuration
    builder.Services.AddDbContext<YuzzContext>(opt =>
        opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

    // JWT Configuration 
    var key = builder.Configuration["JWT_TOKEN"]!;
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = builder.Configuration["Jwt:Audience"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromSeconds(30)
            };
        });

    builder.Services.AddAuthorization();

    builder.Services.AddRazorPages();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
}