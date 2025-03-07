var builder = WebApplication.CreateBuilder(args);

builder.ConfigureSerilog();  // Configure Serilog before anything else.

var config = builder.Configuration;

// Add services to the container.
builder.Services.AddDaemonConfigurations(config);
builder.Services.AddDaemonServices();

builder.WebHost.ConfigureAsSystemdService(config);


builder.Services.AddControllers();


builder.Services.AddLocalHostCors();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
