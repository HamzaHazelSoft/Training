using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Training.Extensions;
using Training.Helper;
using Training.Mappings;
using Training.Models;
using Training.Services;
using Training.Services.Auth;
using Training.Services.Jwt;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

// Register the DbContext with the connection string from appsettings.json
builder.Services.AddDbContext<TrainingContext>(options =>
    options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]));

// Register Identity services with the User model and IdentityRole
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<TrainingContext>()
    .AddDefaultTokenProviders(); 

// Register AutoMapper which help us to map data from DTO to Model and vice versa
builder.Services.AddAutoMapper(typeof(UserMapping));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<IUserService,UserService>(); //Framework inject UserService class when IUserService is requested
builder.Services.AddScoped<IAuthService,AuthService>();


// Configure custom response for model validation errors
builder.Services.ValidateInvalidModel();

//To verify JWT token 
builder.Services.AddJwtAuthentication(builder.Configuration); //Extension Method
                                                              //Internally it calls JwtExtension.AddJwtAuthentication(builder.services,builder.Configuration)

//Identity Password Settings
builder.Services.Configure(options =>
{
    options.Password.RequiredLength = 8; // Change length
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false; // Allow no special characters
    options.Password.RequiredUniqueChars = 1;
});


builder.Services.AddScoped<ITokenService,TokenService>();

var app = builder.Build();

//SeedRoles
await app.SeedRoles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); 
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.MapGet("/", () => "Server Running");

app.Run();
