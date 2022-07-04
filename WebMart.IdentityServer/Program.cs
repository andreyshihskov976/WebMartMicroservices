using WebMart.IdentityServer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var isBuilder = builder.Services.AddIdentityServer()
    .AddInMemoryApiScopes(Config.ApiScopes)
    .AddInMemoryClients(Config.Clients);

isBuilder.AddDeveloperSigningCredential();


// builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

// app.UseHttpsRedirection();

app.UseDeveloperExceptionPage();

app.UseIdentityServer();

// app.UseAuthorization();

// app.MapControllers();

app.Run();
