using FastEndpoints;
using FastEndpoints.Swagger;

using Microsoft.EntityFrameworkCore;

using Webb_Labb02_version2_ApiAndBlazor.Api.Data;
using Webb_Labb02_version2_ApiAndBlazor.Api.Repositories.Implementations;
using Webb_Labb02_version2_ApiAndBlazor.Api.Repositories.Interfaces;

using Webb_Labb02_version2_ApiAndBlazor.Api.DataSeed;

// F�r autentisering:
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FastEndpoints.Security;
using NSwag.AspNetCore;
using Webb_Labb02_version2_ApiAndBlazor.Api;

using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Options;
using NSwag;

using System.Text.Json.Serialization;

// F�r Blazor
using Microsoft.AspNetCore.Cors;




var builder = WebApplication.CreateBuilder(args);

// F�r autentisering:
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"];

// Kontroll f�r att hitta varf�r jag f�r felmeddelande n�r jag f�rs�ker k�ra min app.
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


// Add services to the container.
//builder.Services.AddFastEndpoints();


// L�gg till FastEndpoints och Swagger
//builder.Services
//    .AddFastEndpoints()
//    .SwaggerDocument(options =>
//    {
//        options.DocumentSettings = s =>
//        {
//            s.Title = "WebbLabb02 API";
//            s.Version = "v1";
//        };

//        options.AddAuth("Bearer", new()
//        {
//            Type = NSwag.OpenApiSecuritySchemeType.Http,
//            Scheme = "bearer",
//            BearerFormat = "JWT",
//            In = NSwag.OpenApiSecurityApiKeyLocation.Header,
//            Name = "Authorization",
//            Description = "Ange 'Bearer' f�ljt av ditt JWT-token."
//        });
//    });





//Byter ut builder.Services.SwaggerDocument(); mot nedanst�ende f�r att kunna testa JWT i Swagger:
/*
builder.Services.SwaggerDocument(config =>
{
    config.DocumentSettings = s =>
    {
        s.Title = "WebbLabb02 API";
        s.Version = "v1";
    };

    config.AddAuth("Bearer", new()
    {
        Type = "http",
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header
    });
});
*/



/*
builder.Services.SwaggerDocument(o => {
    o.DocumentSettings = s =>
    {
        s.Title = "WebbLabb02 API";
        s.Version = "v1";
    };
});
*/


/*
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "WebbLabb02 API",
        Version = "v1"
    });

    // L�gg till s�kerhetsschemat
    c.AddSecurityDefinition("Bearer", SwaggerAuth.JwtScheme);
    c.AddSecurityRequirement(SwaggerAuth.JwtRequirement);
});
*/

/* AddSwaggerGen verkar vara fr�n Swashbuckle.AspNetCore, som inte fungerar med FastEndpoints.
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "WebbLabb02 API",
        Version = "1.0.0"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Ange 'Bearer <token>'"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            Array.Empty<string>()
        }
    });
});
*/

builder.Services
    .AddFastEndpoints()
    .SwaggerDocument(config =>
    {
        config.DocumentSettings = s =>
        {
            s.DocumentName = "v1";
            s.Title = "WebbLabb02 API";
            s.Version = "v1";

            s.AddAuth("Bearer", new()
            {
                Type = OpenApiSecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                BearerFormat = "JWT",
                //In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                Name = "Authorization"
            });
        };

        
    });


        /*
        config.AddAuth("Bearer", new()
        {
            Type = "http",
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            Name = "Authorization"
        });
        */








builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();




// F�r att visa min enum ProductStatus som text och inte som siffror i Swagger.
builder.Services.AddControllers()
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });




// F�r Blazor:
// L�gger till CORS f�r att kunna anropa mina API:er fr�n Blazor.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient", policy =>
    {
        policy.WithOrigins("https://localhost:7272")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});










var app = builder.Build();




/* Detta verkar anv�nda Swashbuckle.AspNetCore.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebbLabb02 API v1");
});
*/

// F�r Blazor:
app.UseCors("AllowBlazorClient");

app.UseAuthentication();
app.UseAuthorization();
app.UseFastEndpoints(c =>
{
    c.Versioning.Prefix = "v";
    c.Versioning.PrependToRoute = true;
    c.Serializer.Options.PropertyNamingPolicy = null;
}).UseSwaggerGen(); // UseSwaggerGen var enligt https://fast-endpoints.com/docs/swagger-support#configuration







// L�gger in data i databasen
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    DatabaseSeeder.Seed(db);
}





app.Run();
