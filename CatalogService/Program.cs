using CatalogService.Data;
using CatalogService.Models;
using CatalogService.Repos;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
Console.WriteLine("--> Using InMemory Database");
builder.Services.AddDbContext<CatalogDbContext>(opt => opt.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=ToDoList_v.2_DB;Trusted_Connection=True;"));
builder.Services.AddScoped<IRepository<Category>, CategoryRepo>();
builder.Services.AddScoped<IRepository<SubCategory>, SubCategoryRepo>();
builder.Services.AddScoped<IRepository<Product>, ProductRepo>();
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
