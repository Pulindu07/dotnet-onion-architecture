using MyApp.Application;
using MyApp.Infrastructure;
using MyApp.Infrastructure.Hubs;

var builder = WebApplication.CreateBuilder(args);

var appSettings = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .Build();

builder.Services.ConfigureApplication();
builder.Services.ConfigureInfrastructure(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
builder.Services.AddHttpClient();

// Update CORS configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        builder =>
        {
            builder
                .WithOrigins(
                    "https://snappedlk.com",  // Replace with your actual production domain
                    "http://127.0.0.1:5173"                // Keep for development
                )
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();                       // Required for SignalR
        });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.MigrateDatabase();
}

// Important: Use CORS before mapping routes
app.UseCors("AllowReactApp");

// Configure SignalR
app.MapHub<ChatHub>("/chat-bot");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();