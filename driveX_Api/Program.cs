using driveX_Api.DataBase.DBContexts;
using driveX_Api.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(Program));

var config = builder.Configuration;
var driveXCS = config.GetConnectionString("driveX");
builder.Services.AddDbContext<DriveXDBC>(options=>options.UseSqlServer(driveXCS));

//setting jwt properties
Environment.SetEnvironmentVariable("JWT_KEY", "a1b2c3d4e5f6789012345678901234567890abcdef1234567890abcdef123456");



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
