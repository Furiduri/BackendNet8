using GCatcode.Api.Configuration;
using GCatcode.DataBase;
using GCatcode.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// --- Configuration binding ---
var connectionStrings = builder.Configuration
    .GetSection("ConnectionStrings")
    .Get<ConnectionStrings>() ?? new ConnectionStrings();

var jwtSettings = builder.Configuration
    .GetSection("Jwt")
    .Get<JwtSettings>() ?? new JwtSettings();

var appSettingsConfig = builder.Configuration
    .GetSection("AppSettings")
    .Get<AppSettings>() ?? new AppSettings();

appSettingsConfig.DB = connectionStrings;

builder.Services.AddSingleton(appSettingsConfig);

// Add DbContext
builder.Services.AddDbContext<AppDBContext>(dbContext =>
{
    dbContext.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Register Dapper/SQL Connection
builder.Services.AddScoped<SqlConnection>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    return Settings.GetSqlDBConnection(config);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("MyAllowSpecificOrigins", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "https://localhost", "https://anotherdomain.com")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Add Auth
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = builder.Environment.IsProduction(),
            ValidateAudience = builder.Environment.IsProduction(),
            ValidateLifetime = true,
            ValidateIssuerSigningKey = builder.Environment.IsProduction(),
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });
builder.Services.AddAuthorization();
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// --- CORS ---
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", corsBuilder =>
    {
        corsBuilder.WithOrigins(
            "http://localhost:5173",
            "https://localhost:5173")
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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(name: "default",
               pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();