using ChatApi.Api.Extension;
using ChatApi.Application.handler;
using ChatApi.Application.ServiceRegistration;
using ChatApi.Infrastructure.Hubs;
using ChatApi.Infrastructure.Identity;
using ChatApi.Infrastructure.InfrastructureRegistration;
using ChatApi.Infrastructure.JWT;
using ChatApi.Infrastructure.presistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();

builder.Services.AddHttpContextAccessor();

builder.Services.AddInfrastructureRegistration();

builder.Services.AddApplicationRegistration();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("MyCon")));

builder.Services.AddIdentity<ApplicationUser, Microsoft.AspNetCore.Identity.IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddJwtConfiguration(builder.Configuration);
builder.Services.AddSwaggerConfig();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://127.0.0.1:5500") // ضع هنا Origin الذي تستخدمه
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();

app.UseAuthentication(); 
app.UseAuthorization();
app.UseStaticFiles();
app.MapControllers();
app.MapHub<ChatApi.Infrastructure.Hubs.ChatHub>("/chatHub");
    

app.Run();
