using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace DoughBro.src.Extensions;

public static class AuthenticationExtensions
{
    public static IServiceCollection AddFirebaseAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var firebaseProjectId = configuration["Firebase:project_id"];

        if (string.IsNullOrEmpty(firebaseProjectId))
        {
            throw new InvalidOperationException("Firebase:ProjectId is missing from configuration.");
        }

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = $"https://securetoken.google.com/{firebaseProjectId}";
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = $"https://securetoken.google.com/{firebaseProjectId}",
                    ValidateAudience = true,
                    ValidAudience = firebaseProjectId,
                    ValidateLifetime = true
                };
            });

        services.AddAuthorization();

        return services;
    }
}

