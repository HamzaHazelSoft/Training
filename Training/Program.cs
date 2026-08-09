using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserManagementSystem.Extensions;
using UserManagementSystem.Mappings;
using UserManagementSystem.Models;
using UserManagementSystem.Repositories;
using UserManagementSystem.Services;
using UserManagementSystem.Services.Auth;
using UserManagementSystem.Services.Auth.Implementation;
using UserManagementSystem.Services.Jwt;
using UserManagementSystem.Services.Jwt.Implementation;
using UserManagementSystem.Services.Mail;
using UserManagementSystem.Services.Mail.Implementation;
using UserManagementSystem.Services.Users.Implementation;
using DbContext = UserManagementSystem.Context.DbContext;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();


// Register the application's database context with SQL Server.
builder.Services.AddDbContext<UserManagementSystem.Context.DbContext>(options =>
    options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]));

// Register Identity services with the User model and IdentityRole
builder.Services.AddIdentity<User, IdentityRole>(options =>
    {
        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<DbContext>()
    .AddDefaultTokenProviders();

// Register AutoMapper for mapping between entities and DTOs.
builder.Services.AddAutoMapper(typeof(UserMapping));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register application services.
builder.Services.AddScoped<IUserService,UserService>();
builder.Services.AddScoped<IAuthService,AuthService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddTransient<IMailService, MailService>();


// Configure custom response for model validation errors Extension
builder.Services.ValidateInvalidModel();

// Configure JWT authentication and token validation
//Internally it calls JwtExtension.AddJwtAuthentication(builder.services,builder.Configuration)
builder.Services.AddJwtAuthentication(builder.Configuration);

// Configure ASP.NET Core Identity password requirements.
builder.Services.ConfigurePassword();

var app = builder.Build();

// Configure development-only middleware and tools.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.MapGet("/", () => "Server Running");

app.Run();
