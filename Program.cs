using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DotnetAPI.Data;



var builder = WebApplication.CreateBuilder(args);
Configure();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(options => 
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.MapRazorPages();


app.Run();


void Configure()
{
    IConfigurationSection Dbconfig = builder.Configuration.GetSection("ConnectionStrings");
    string? DefaulConection = Dbconfig.GetSection("DefaultConnection").Value;

    if (string.IsNullOrWhiteSpace(DefaulConection))
    {
        throw new ArgumentNullException();
    }

    builder.Services.AddDbContextPool<YuzzContext>(options =>
    {
        options.UseSqlite(DefaulConection);
    });

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
}

