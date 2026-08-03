using Microsoft.EntityFrameworkCore;
using Training.Mappings;
using Training.Models;
using Training.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

// Register the DbContext with the connection string from appsettings.json
builder.Services.AddDbContext<TrainingContext>(options =>
    options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]));

// Register AutoMapper which help us to map data from DTO to Model and vice versa
builder.Services.AddAutoMapper(typeof(UserMapping));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<IUserService,UserService>(); //Framework inject UserService class when IUserService is requested

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.MapGet("/", () => "Server Running");

app.Run();
