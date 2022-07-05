using WebMart.IdentityServer;

var builder = WebApplication.CreateBuilder(args);

var isBuilder = builder.Services.AddIdentityServer()
    .AddInMemoryIdentityResources(Config.IdentityResources)
    .AddInMemoryApiScopes(Config.ApiScopes)
    .AddInMemoryClients(Config.Clients);

isBuilder.AddDeveloperSigningCredential();

var app = builder.Build();

app.UseIdentityServer();

app.Run();
