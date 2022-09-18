using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using TodoList.Api.Common.Hosting.ConfigSections;

namespace TodoList.Api.Common.Hosting.Extensions;

public static class ServiceCollectionExtensions
{
    public static OptionsBuilder<CorsOptions> ConfigureCors(this IServiceCollection services, string corsPolicy)
    {
        return services.AddOptions<CorsOptions>()
            .Configure<CorsConfig>((options, corsConfig) =>
            {
                options.AddPolicy(name: corsPolicy,
                    policy =>
                    {
                        policy.WithOrigins(corsConfig!.AllowOrigins!.Split(","))
                            .SetIsOriginAllowedToAllowWildcardSubdomains()
                            .AllowAnyHeader()
                            .WithMethods(HttpMethods.Get, HttpMethods.Head, HttpMethods.Options, HttpMethods.Post, HttpMethods.Put, HttpMethods.Delete)
                            .WithExposedHeaders(corsConfig.ExposedHeaders!.Split(","))
                            .SetPreflightMaxAge(TimeSpan.FromMinutes(10));
                    });
            });

    }

    public static IServiceCollection ConfigureHsts(this IServiceCollection services)
    {
        return services.AddHsts(options =>
        {
            options.Preload = true;
            options.IncludeSubDomains = true;
            options.MaxAge = TimeSpan.FromDays(7);
        });
    }
    
    public static IServiceCollection ConfigureSwaggerGeneration(this IServiceCollection services)
    {
        return services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo {Title = "TodoList.Api", Version = "v1"});
            options.TagActionsBy(api => new[] {api.GroupName ?? "Un-grouped"});
            options.DocInclusionPredicate((_, api) => !string.IsNullOrWhiteSpace(api.GroupName));

        });
    }
    
    public static IServiceCollection AddConfigs(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddCorsConfig(configuration);
    }
    
    public static IServiceCollection AddCorsConfig(this IServiceCollection services, IConfiguration configuration)
    {
        return services.Configure<CorsConfig>(configuration.GetSection("Cors"))
            .AddTransient(s => s.GetRequiredService<IOptionsMonitor<CorsConfig>>().CurrentValue);
    }
}