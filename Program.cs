using ColllaberaDigital.WebApi.DependencyServices.Contract;
using ColllaberaDigital.WebApi.DependencyServices.Services;
using ColllaberaDigital.WebApi.Policies;
using System.ComponentModel;
using System.Net;

var builder = WebApplication.CreateBuilder(args);
var container = new CookieContainer();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
 builder.Services.AddMemoryCache();

builder.Services.AddHttpClient<IStoriesServices, StoriesServices>(client =>
                {
                    client.BaseAddress = new Uri(builder.Configuration["BaseUrl"]);
                })
                .ConfigurePrimaryHttpMessageHandler(() =>
                {
                    HttpClientHandler httpClientHandler = new HttpClientHandler()
                    {
                        CookieContainer = container,
                        UseCookies = true
                    };
                    return httpClientHandler;
                })
                .AddPolicyHandler(HttpPolices.GetRetryPolicy())
                .AddPolicyHandler(HttpPolices.GetCircuitBreakerPolicy()); ;



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
