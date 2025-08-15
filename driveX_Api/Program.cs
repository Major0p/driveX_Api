using driveX_Api.DataBase.DBContexts;
using driveX_Api.DTOs.JWT;
using driveX_Api.Repository.Auth;
using driveX_Api.CommonClasses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;
using driveX_Api.Repository.File;
using Microsoft.IdentityModel.Tokens.Experimental;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(configAction => configAction.AddMaps(Assembly.GetEntryAssembly()));

var config = builder.Configuration;
var driveXCS = config.GetConnectionString("driveX");
builder.Services.AddDbContext<DriveXDBC>(options=>options.UseSqlServer(driveXCS));

builder.Services.AddScoped<IAuthentication, AuthenticationService>();
builder.Services.AddScoped<IFileSave,FileSaveServices>();
builder.Services.AddScoped<IJwtToken, JwtTokenServices>();


builder.Services.Configure<JwtToken>("AccessToken",config.GetSection("AccessToken"));
builder.Services.Configure<JwtToken>("SessionToken", config.GetSection("SessionToken"));

var accessToken = config.GetSection("AccessToken").Get<JwtToken>();
var sessionToken = config.GetSection("SessionToken").Get<JwtToken>();

builder.Services.AddAuthentication( options=>
{
    options.DefaultAuthenticateScheme = "AccessScheme";
    options.DefaultChallengeScheme = "AccessScheme";
})
    .AddJwtBearer("AccessScheme", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = accessToken.Issuer,
            ValidAudience = accessToken.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(accessToken.Key)),
            ClockSkew = TimeSpan.Zero
        };
    })
    .AddJwtBearer("SessionScheme", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = sessionToken.Issuer,
            ValidAudience = sessionToken.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(sessionToken.Key)),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
    policy => policy.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAllOrigins");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
