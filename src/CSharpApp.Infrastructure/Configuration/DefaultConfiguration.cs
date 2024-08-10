namespace CSharpApp.Infrastructure.Configuration;

public static class DefaultConfiguration
{
    public static IServiceCollection AddDefaultConfiguration(this IServiceCollection services)
    {
        services.AddScoped<ITodoService, TodoService>();
        
        return services;
    }
}