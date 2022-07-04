using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebMart.Extensions.AsyncDataServices;
using WebMart.Extensions.EventProcessing;
using WebMart.Extensions.SyncDataServices;
using WebMart.Microservices.OrdersService.Data;
using WebMart.Microservices.OrdersService.EventProcessing;
using WebMart.Microservices.OrdersService.Repos;
using WebMart.Microservices.OrdersService.Repos.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = builder.Configuration
            .GetSection("IdentityParameters")
            .GetValue<string>("IdentityServerHost");
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false
        };
    });

builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("ApiScope", policy =>
        {
            policy.RequireAuthenticatedUser();
            policy.RequireClaim
            (
                "scope",
                builder.Configuration
                    .GetSection("IdentityParameters")
                    .GetValue<string>("Scope")
            );
        });
    });

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
