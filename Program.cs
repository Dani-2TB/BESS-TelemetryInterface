using Microsoft.EntityFrameworkCore;
using DotnetAPI.Data;
using DotnetAPI.Models;
using DotNetEnv;
using DotNetEnv.Configuration;

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

    builder.Services.AddAuthorization();

    builder.Services.AddRazorPages();
    builder.Services.AddHttpClient(); 

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var configuration = new ConfigurationBuilder()
        .AddDotNetEnv(".env", LoadOptions.TraversePath())
        .Build();
}