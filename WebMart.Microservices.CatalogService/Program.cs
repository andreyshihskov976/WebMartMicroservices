using IdentityModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebMart.Extensions.AsyncDataServices;
using WebMart.Microservices.CatalogService.Data;
using WebMart.Microservices.CatalogService.Repos;
using WebMart.Microservices.CatalogService.Repos.Interfaces;

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
            ValidateAudience = false,
        };
    });

builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("m2m.communication", policy =>
        {
            policy.RequireAuthenticatedUser();
            policy.RequireClaim
            (
                JwtClaimTypes.Scope,
                builder.Configuration
                    .GetSection("IdentityParameters:Scope").Value
            );
        });
        options.AddPolicy("admins_only", policy =>
        {
            policy.RequireAuthenticatedUser();
            policy.RequireClaim(JwtClaimTypes.Scope,"admin_permissions");
        });
        options.AddPolicy("users_allowed", policy =>
        {
            policy.RequireAuthenticatedUser();
            policy.RequireClaim(JwtClaimTypes.Scope,"user_permissions");
        });
    });

builder.Services.AddDbContext<CatalogDbContext>(opt =>
    opt.UseNpgsql(
        builder.Configuration.GetValue<string>("DbConnection")
    ));

builder.Services.AddScoped<ICategoryRepo, CategoryRepo>();
builder.Services.AddScoped<ISubCategoryRepo, SubCategoryRepo>();
builder.Services.AddScoped<IProductRepo, ProductRepo>();

builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

DbInitializer.PrepPopulation(app, builder.Environment.IsProduction());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
