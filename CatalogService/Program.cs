using System.Text.Json.Serialization;
using CatalogService.Data;
using CatalogService.Repos;
using CatalogService.Repos.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<CatalogDbContext>(opt =>
    opt.UseNpgsql(
        builder.Configuration.GetValue<string>("DbConnection")
    ));

builder.Services.AddScoped<ICategoryRepo, CategoryRepo>();
builder.Services.AddScoped<ISubCategoryRepo, SubCategoryRepo>();
builder.Services.AddScoped<IProductRepo, ProductRepo>();

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

app.UseAuthorization();

app.MapControllers();

app.Run();
