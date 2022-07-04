using Microsoft.EntityFrameworkCore;
using WebMart.Extensions.SyncDataServices;
using WebMart.Extensions.AsyncDataServices;
using WebMart.Microservices.BasketService.Data;
using WebMart.Microservices.BasketService.EventProcessing;
using WebMart.Microservices.BasketService.Repos;
using WebMart.Microservices.BasketService.Repos.Interfaces;
using WebMart.Extensions.EventProcessing;
using Microsoft.IdentityModel.Tokens;

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

builder.Services.AddDbContext<BasketDbContext>(opt =>
    opt.UseNpgsql(
        builder.Configuration.GetValue<string>("DbConnection")
    ));

builder.Services.AddScoped<IBasketRepo, BasketRepo>();
builder.Services.AddScoped<IProductRepo, ProductRepo>();

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

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// app.UseEndpoints(endpoints =>
// {
//     endpoints.MapControllers()
//         .RequireAuthorization("ApiScope");
// });

// app.UseHttpsRedirection();

app.MapControllers();

app.Run();
