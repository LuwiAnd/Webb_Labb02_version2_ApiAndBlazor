using Microsoft.OpenApi.Models;

namespace Webb_Labb02_version2_ApiAndBlazor.Api;

public class SwaggerAuth
{
    public static OpenApiSecurityScheme JwtScheme => new()
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Ange 'Bearer <token>'"
    };

    public static OpenApiSecurityRequirement JwtRequirement => new()
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
    };
}
