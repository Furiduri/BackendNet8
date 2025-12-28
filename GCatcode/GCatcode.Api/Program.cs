using GCatcode.Repository.DB.UserRolServices;
using GCatcode.Repository.DB.UserServices;
using GCatcode.SQLServerDatabase;
using GCatcode.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Data.SqlClient;
using System.Text;
using GCatcode.Repository.DB.RolServices;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext
builder.Services.AddDbContext<AppDBContext>(dbContext =>
{
    dbContext.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Register Dapper/SQL Connection
builder.Services.AddScoped<SqlConnection>(sp => {
    var config = sp.GetRequiredService<IConfiguration>();
    return Settings.GetSqlDBConnection(config);
});

// Register Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRolService, UserRolService>();
builder.Services.AddScoped<IRolesService, RolesService>();

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();    
}

app.UseHttpsRedirection();
app.UseCors("MyAllowSpecificOrigins"); // Apply the named policy here
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(name: "default",
               pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();