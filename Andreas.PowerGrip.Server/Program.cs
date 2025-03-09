var builder = WebApplication.CreateBuilder(args);

// Add and configure Serilog before anything else!
builder.ConfigureSerilog();

// Here are the main thingies
var config = builder.Configuration;

Log.Debug("Parsing environment variables from .env");
var envVars = EnvLoader.Load(".env");  // Load my environment variables
foreach (var (key, value) in envVars)
{
    Log.Debug("{key} = {value}", key, value);
    Environment.SetEnvironmentVariable(key, value);
}

// Add services to the container.
builder.Services.AddControllers();

// Add custom conventions.
builder.ConfigureConventions();

// Add storage services (databases)
builder.Services.AddStorage(config);

// Add support to serve our frontend
builder.Services.AddFrontEnd(config);

// Add our services
builder.Services.AddPowerGripServices();

// Add app configs
builder.Services.AddPowerGripConfigurations(config);

// Add JWT Auth and configure it
builder.Services.AddPowerGripJwtAuth(config);

// Configuring swagger
builder.Services.AddAndConfigureSwagger();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
var isDevEnvironment = app.Environment.IsDevelopment();

if (isDevEnvironment)
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();

app.MapStaticAssets();
app.UseSpaStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.UsePowerGripMiddlewares();

app.MapControllers();

app.UseFrontEnd();

app.SeedData();

app.Run();
