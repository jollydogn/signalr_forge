using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SignalForge.EntityFrameworkCore;
using SignalForge.Extensions;
using SignalForge.Filters;
using SignalForge.Sample;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(options =>
{
    // Register global request logging filter
    options.Filters.Add<RequestLoggingFilter>();
    
    // Register global activity logging filter (triggered via attributes)
    options.Filters.Add<ActivityLoggingFilter>();
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Register the SampleDbContext with PostgreSQL
builder.Services.AddDbContext<SampleDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// IMPORTANT: Bind your ISignalForgeDbContext to the SampleDbContext
builder.Services.AddScoped<ISignalForgeDbContext>(provider => provider.GetRequiredService<SampleDbContext>());

// Register SignalForge core services
builder.Services.AddSignalForge(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseRouting();

// Use authentication and authorization BEFORE mapping the hub
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Map the SignalForge Hub endpoints
app.MapSignalForgeHub();

// Run migrations on startup (for demo purposes)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SampleDbContext>();
    // Note: ensure you create migrations first: 'dotnet ef migrations add Initial'
    // db.Database.Migrate(); 
}

app.Run();
