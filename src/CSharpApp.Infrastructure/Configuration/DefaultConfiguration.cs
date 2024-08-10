using Microsoft.Extensions.Configuration;

namespace CSharpApp.Infrastructure.Configuration;

public static class DefaultConfiguration
{
	// ---- TodoService ----
	public static IServiceCollection AddDefaultConfiguration(
		this IServiceCollection services,
		IConfiguration configuration)
	{
		services.AddHttpClient<TodoService>(
			client =>
			{
				string? baseUrl = configuration["BaseUrl"];
				if (baseUrl == null)
				{
					throw new ArgumentException(nameof(baseUrl));
				}
				client.BaseAddress = new Uri(baseUrl);
			});

		// Use a factory method to create ITodoService using the configured TodoService
		services.AddScoped<ITodoService>(sp =>
		{
			var httpClient = sp
				.GetRequiredService<IHttpClientFactory>()
				.CreateClient(nameof(TodoService));
			return ActivatorUtilities.CreateInstance<TodoService>(sp, httpClient);
		});


		// ---- PostsService ----
		services.AddHttpClient<PostsService>(
			client =>
			{
				string? baseUrl = configuration["BaseUrl"];
				if (baseUrl == null)
				{
					throw new ArgumentException(nameof(baseUrl));
				}
				client.BaseAddress = new Uri(baseUrl);
			});

		// Use a factory method to create IPostsService using the configured PostsService
		services.AddScoped<IPostsService>(sp =>
		{
			var httpClient = sp
				.GetRequiredService<IHttpClientFactory>()
				.CreateClient(nameof(PostsService));
			return ActivatorUtilities.CreateInstance<PostsService>(sp, httpClient);
		});

		return services;
	}
}