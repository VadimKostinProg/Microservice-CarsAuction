using MongoDB.Driver;
using MongoDB.Entities;
using Polly;
using Polly.Extensions.Http;
using SearchService.Data;
using SearchService.ServiceContracts;
using SearchService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddHttpClient<IAuctionsService>().AddPolicyHandler(GetPolicy());

builder.Services.AddScoped<IAuctionsService, AuctionsService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Lifetime.ApplicationStarted.Register(async () =>
{
    try
    {
        await app.InitDB();
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }
});


app.Run();

static IAsyncPolicy<HttpResponseMessage> GetPolicy() =>
    HttpPolicyExtensions.HandleTransientHttpError()
                        .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                        .WaitAndRetryForeverAsync(_ => TimeSpan.FromSeconds(3));