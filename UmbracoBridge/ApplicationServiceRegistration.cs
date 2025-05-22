using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Net.Http.Headers;
using UmbracoBridge.DelegatingHandlers;
using UmbracoBridge.Models;
using UmbracoBridge.Service;
using UmbracoBridge.Service.Interfaces;

namespace UmbracoBridge
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            UmbracoCMSSettings settings = new();
            configuration.GetSection(UmbracoCMSSettings.SettingNameSection).Bind(settings);

            services.AddOptions<UmbracoCMSSettings>()
                .BindConfiguration(UmbracoCMSSettings.SettingNameSection)
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddScoped<IDocumentTypeService, DocumentTypeService>();
            services.AddScoped<IHealthService, HealthService>();
            services.AddTransient<AuthenticationHandler>();

            services.AddHttpClient("umbraco", (serviceProvider, client) =>
            {
                client.BaseAddress = new Uri(settings.BaseUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            })
            .AddHttpMessageHandler<AuthenticationHandler>();

            services.AddHealthChecks()
            .AddCheck<HealthService>("umbraco-cms-check", HealthStatus.Unhealthy);

            return services;
        }
    }
}
