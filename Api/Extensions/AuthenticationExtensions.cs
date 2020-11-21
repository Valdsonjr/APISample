using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;

namespace Api.Extensions
{
    /// <summary>
    /// Extensões de autenticação
    /// </summary>
    public static class AuthenticationExtensions
    {
        /// <summary>
        /// Configura a autenticação JWT
        /// </summary>
        /// <param name="services"></param>
        public static void AddCustomAuthentication(this IServiceCollection services)
        {
            var key = Environment.GetEnvironmentVariable("JWT_SECRET")
                ?? throw new InvalidOperationException("Couldn't find JWT_SECRET in your environment variables.");

            var issuer = Environment.GetEnvironmentVariable("JWT_ISSUER")
                ?? throw new InvalidOperationException("Couldn't find JWT_ISSUER in your environment variables.");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
             options.TokenValidationParameters = new TokenValidationParameters
             {
                 ValidateIssuer = true,
                 ValidIssuer = issuer,
                 ValidateAudience = false,
                 IssuerSigningKey = new SymmetricSecurityKey(Base64UrlEncoder.DecodeBytes(key))
             });
        }
    }
}
