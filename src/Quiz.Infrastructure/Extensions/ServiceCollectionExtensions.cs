namespace Quiz.Infrastructure.Extensions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ApplicationInsights.Extensibility;
using Quiz.Infrastructure.Authentication;
using Quiz.Infrastructure.Telemetry;
using Quiz.Shared.Authentication;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Authorization;
using Microsoft.OpenApi.Models;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        // Add Keycloak authentication
        services.AddKeycloakWebApiAuthentication(configuration);

        // Add Keycloak authorization with policies
        services.AddAuthorization()
            .AddKeycloakAuthorization(configuration)
            .AddAuthorizationBuilder()
            .AddPolicy("Admin:Read", policy =>
                policy.RequireRealmRoles("quiz-admin:read"))
            .AddPolicy("Admin:Write", policy =>
                policy.RequireRealmRoles("quiz-admin:write"))
            .AddPolicy("Admin:Manage", policy =>
                policy.RequireRealmRoles("quiz-admin:read", "quiz-admin:write"));

        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUser, CurrentUser>();
        services.AddScoped<ISubscriptionService, SubscriptionService>();

        services.AddApplicationInsightsTelemetry();
        services.AddSingleton<ITelemetryInitializer, CloudRoleTelemetryInitializer>();

        return services;
    }
    
    public static IServiceCollection AddAzureAppConfiguration(this IServiceCollection services, ConfigurationManager configuration)
    {
        var appConfigConnectionString = configuration["AppConfig:ConnectionString"];
        var copyOfSources = configuration.Sources.ToList();
        if (string.IsNullOrWhiteSpace(appConfigConnectionString) is false)
        {
            configuration.AddAzureAppConfiguration(o =>
            {
                o.Connect(appConfigConnectionString);
                o.Select(KeyFilter.Any, LabelFilter.Null);

                // config label selection.
                var labels = configuration["AppConfig:Labels"]?.Split(',') ?? [];
                foreach (var label in labels)
                    o.Select(KeyFilter.Any, label);
            });

            if (configuration.GetValue("AppConfig:ReOrderSources", false))
            {
                var secretsSource = configuration.Sources
                    .FirstOrDefault(t => t is JsonConfigurationSource jsonConfigurationSource && jsonConfigurationSource.Path?.EndsWith("secrets.json") is true);
                configuration.Sources.Add(secretsSource!);
            }
        }

        return services;
    }
} 