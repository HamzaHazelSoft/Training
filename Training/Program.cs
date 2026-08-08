using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Training.Context;
using Training.Extensions;
using Training.Helper;
using Training.Mappings;
using Training.Models;
using Training.Repositories;
using Training.Services;
using Training.Services.Auth;
using Training.Services.Auth.Implementation;
using Training.Services.Jwt;
using Training.Services.Jwt.Implementation;
using Training.Services.Mail;
using Training.Services.Mail.Implementation;
using Training.Services.Users.Implementation;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

// Registering the DBContext
builder.Services.AddDbContext<TrainingContext>(options =>
    options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]));

builder.Services.AddHangfire(config => //Configuring Hangfire
    config.UseSqlServerStorage(
        builder.Configuration["ConnectionStrings:DefaultConnection"]
    ));

builder.Services.AddHangfireServer(); //Background jobs execute karne wala Hangfire worker start kar rha

// Register Identity services with the User model and IdentityRole
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<TrainingContext>()
    .AddDefaultTokenProviders(); //Identity ko security tokens generate/validate karne ka mechanism deta hai.

// Register AutoMapper which help us to map data from DTO to Model and vice versa
builder.Services.AddAutoMapper(typeof(UserMapping));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<IUserService,UserService>(); //Framework inject UserService class when IUserService is requested
builder.Services.AddScoped<IAuthService,AuthService>();


// Configure custom response for model validation errors Extension
builder.Services.ValidateInvalidModel();

//Configure
builder.Services.AddJwtAuthentication(builder.Configuration); //Extension Method
                                                              //Internally it calls JwtExtension.AddJwtAuthentication(builder.services,builder.Configuration)

//Identity Password Settings Extension
builder.Services.ConfigurePassword();


builder.Services.AddScoped<ITokenService,TokenService>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddTransient<IMailService, MailService>();

var app = builder.Build();

//SeedRoles Extension
await app.SeedRoles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); 
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHangfireDashboard();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.MapGet("/", () => "Server Running");

app.Run();
