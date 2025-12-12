using ChatApi.Api.Extension;
using ChatApi.Application.ServiceRegistration;
using ChatApi.Infrastructure.Hubs;
using ChatApi.Infrastructure.Identity;
using ChatApi.Infrastructure.InfrastructureRegistration;
using ChatApi.Infrastructure.JWT;
using ChatApi.Infrastructure.presistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.WriteIndented = true;
    });

builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = builder.Environment.IsDevelopment();
    options.MaximumReceiveMessageSize = 102400; 
    options.StreamBufferCapacity = 10;
});


builder.Services.AddHttpContextAccessor();
builder.Services.AddInfrastructureRegistration();
builder.Services.AddApplicationRegistration();
builder.Services.AddJwtConfiguration(builder.Configuration);
builder.Services.AddSwaggerConfig();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("MyCon")));

builder.Services.AddIdentity<ApplicationUser, Microsoft.AspNetCore.Identity.IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 8;

    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
        .WithOrigins(
            "http://127.0.0.1:5500",
            "http://localhost:3000",
            "http://localhost:5173",
            "http://localhost:5174"
        )
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});

var app = builder.Build();



if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ChatApi v1");
        c.RoutePrefix = string.Empty; 
        c.DocumentTitle = "ChatApi Documentation";
    });
}

app.UseCors("AllowAll");

// app.UseMiddleware<RateLimitingMiddleware>();


app.UseStaticFiles();

app.UseAuthentication();
// app.UseMiddleware<JwtDebugMiddleware>(); // Removed: Middleware not found
app.UseAuthorization();

// app.UseMiddleware<GlobalExceptionMiddleware>();

app.MapControllers();

app.MapHub<ChatHub>("/chatHub");


app.Run();

