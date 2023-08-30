using MongoDB.Driver;
using MongoDB.Entities;
using SearchService.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

try
{
    await app.InitDB();
}
catch(Exception) { }

app.Run();