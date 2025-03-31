using FastEndpoints;
using FastEndpoints.Swagger;

using Microsoft.EntityFrameworkCore;

using Webb_Labb02_version2_ApiAndBlazor.Api.Data;
using Webb_Labb02_version2_ApiAndBlazor.Api.Repositories.Implementations;
using Webb_Labb02_version2_ApiAndBlazor.Api.Repositories.Interfaces;

using Webb_Labb02_version2_ApiAndBlazor.Api.DataSeed;

// För autentisering:
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddFastEndpoints();
builder.Services.SwaggerDocument();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IProductRepository, ProductRepository>();

// För autentisering:
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"];

// Kontroll för att hitta varför jag får felmeddelande när jag försöker köra min app.
if (string.IsNullOrWhiteSpace(secretKey))
    throw new Exception("JWT SecretKey saknas i konfigurationen!");


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();


var app = builder.Build();

/* Detta fungerar inte när man kör Fast Endpoints.
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
*/


app.UseFastEndpoints();
app.UseSwaggerGen();

// För autentisering
app.UseAuthentication();
app.UseAuthorization();

// Lägger in data i databasen
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    DatabaseSeeder.Seed(db);
}


app.Run();
