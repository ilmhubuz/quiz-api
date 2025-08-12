using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Quiz.CSharp.Api.Helper;

namespace Quiz.CSharp.Api.Extensions;

using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Quiz.CSharp.Api.Mapping;
using Quiz.CSharp.Api.Services;
using Quiz.CSharp.Api.Validators;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCSharpApi(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(CollectionProfile).Assembly);

        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssembly(typeof(SubmitAnswerRequestValidator).Assembly);

        services.AddScoped<IAnswerService, AnswerService>();
        services.AddScoped<IAnswerValidator, AnswerValidator>();
        services.AddScoped<ICollectionService, CollectionService>();
        services.AddScoped<IQuestionService, QuestionService>();
        services.AddScoped<IResultsService, ResultsService>();
        services.AddScoped<ISubscriptionGuard, SubscriptionGuard>();
        services.AddScoped<ICollectionManagementService, CollectionManagementService>();
        services.AddScoped<IUserProgressService, UserProgressService>();

        return services;
    }
    
    public static IServiceCollection AddSwaggerWithOAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SchemaFilter<QuestionMetadataSchemaFilter>();
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Quiz Platform API", Version = "v1" });
            
            // Add OAuth2/OIDC authentication for Keycloak
            // In Swagger UI, click "Authorize" and use OAuth2 (oauth2) to authenticate with Keycloak
            c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    Implicit = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri($"{configuration["Keycloak:auth-server-url"]}realms/{configuration["Keycloak:realm"]}/protocol/openid-connect/auth"),
                        TokenUrl = new Uri($"{configuration["Keycloak:auth-server-url"]}realms/{configuration["Keycloak:realm"]}/protocol/openid-connect/token"),
                        Scopes = new Dictionary<string, string>
                        {
                            { "openid", "OpenID Connect scope" },
                            { "profile", "Profile information" },
                            { "email", "Email address" }
                        }
                    }
                }
            });

            // Keep Bearer token as fallback
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "oauth2"
                        }
                    },
                    ["openid", "profile", "email"]
                }
            });
        });

        return services;
    }
} 