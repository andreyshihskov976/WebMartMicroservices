using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebMart.IdentityServer;
using WebMart.IdentityServer.Data;
using WebMart.IdentityServer.Models;
using WebMart.IdentityServer.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

var isBuilder = builder.Services.AddIdentityServer(options =>
{
    options.Events.RaiseErrorEvents = true;
    options.Events.RaiseInformationEvents = true;
    options.Events.RaiseFailureEvents = true;
    options.Events.RaiseSuccessEvents = true;

    // see https://identityserver4.readthedocs.io/en/latest/topics/resources.html
    options.EmitStaticAudienceClaim = true;
})
    .AddInMemoryIdentityResources(Config.IdentityResources)
    .AddInMemoryApiScopes(Config.ApiScopes)
    .AddInMemoryClients(Config.Clients)
    .AddAspNetIdentity<ApplicationUser>();

// not recommended for production - you need to store your key material somewhere secure
isBuilder.AddDeveloperSigningCredential();

var app = builder.Build();

SeedData.EnsureSeedData(builder.Configuration.GetConnectionString("DefaultConnection"));

app.UseIdentityServer();

app.Run();
