using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Training.Mappings;
using Training.Models;
using Training.Services;
using Training.Helper;

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


builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var response = new Response<object>
        {
            Success = false,
            Message = "Validation Failed",
            Data = null,
            Errors = context.ModelState
                .Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList()
        };

        return new BadRequestObjectResult(response);
    };
});

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
