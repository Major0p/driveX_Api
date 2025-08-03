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

//setting jwt properties
builder.Services.AddScoped<IJwtToken, JwtTokenServices>();
builder.Services.Configure<JwtToken>(config.GetSection("JwtToken"));
var jwtToken = config.GetSection("JwtToken").Get<JwtToken>();    
var key = Encoding.UTF8.GetBytes(jwtToken.Key);

builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
 {
     options.TokenValidationParameters = new TokenValidationParameters
     {
         ValidateIssuer = true,
         ValidateAudience = true,
         ValidateLifetime = true,
         ValidateIssuerSigningKey = true,
         ValidIssuer = jwtToken.Issuer,
         ValidAudience = jwtToken.Audience,
         IssuerSigningKey = new SymmetricSecurityKey(key)
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

app.UseAuthorization();

app.MapControllers();

app.Run();
