using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BookApi.Models;
using BookApi.Data;
using BookApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
options.UseSqlite(builder.Configuration.GetConnectionString("Default")));

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<PasswordHasher<User>>();
builder.Services.AddScoped<TokenService>();

builder.Services
.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
        )
    };
});

builder.Services.AddControllers()
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularDev", policy =>
    {
        policy.WithOrigins("http://localhost:4200", "https://booklibrary-app-cris.netlify.app")
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

// builder.Services.AddOpenApi();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.Migrate();
    var passwordHasher = scope.ServiceProvider.GetRequiredService<PasswordHasher<User>>();

    var adminExists = context.Users.Any(u => u.Role == Roles.Admin);
    if (!adminExists)
    {
        var admin = new User
        {
            Username = "Admin",
            Email = "admin@admin.com",
            Role = Roles.Admin,
            PasswordHash = string.Empty
        };
        admin.PasswordHash = passwordHasher.HashPassword(admin, "Password1!");

        context.Users.Add(admin);
        context.SaveChanges();
    }

}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAngularDev");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
