using Microsoft.EntityFrameworkCore;
using WebMart.Microservices.Extensions.AsyncDataServices;
using WebMart.Microservices.Extensions.EventProcessing;
using WebMart.Microservices.Extensions.SyncDataServices;
using WebMart.Microservices.OrdersService.Data;
using WebMart.Microservices.OrdersService.EventProcessing;
using WebMart.Microservices.OrdersService.Repos;
using WebMart.Microservices.OrdersService.Repos.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<OrdersDbContext>(opt =>
    opt.UseNpgsql(
        builder.Configuration.GetValue<string>("DbConnection")
    ));

builder.Services.AddScoped<IBasketRepo,BasketRepo>();
builder.Services.AddScoped<IOrderRepo,OrderRepo>();

builder.Services.AddSingleton<IEventProcessor, EventProcessor>();
builder.Services.AddHostedService<MessageBusSubscriber>();
builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();

builder.Services.AddHttpClient<IHttpDataService, HttpDataService>()
    .ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler
        {
           ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
        });

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
