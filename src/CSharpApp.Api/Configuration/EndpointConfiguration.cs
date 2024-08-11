namespace CSharpApp.Api.Configuration
{
	public static class EndpointConfiguration
	{
		public static void ConfigureEndpoints(this WebApplication app)
		{
			app.ConfigureTodosEndpoints();
			app.ConfigurePostsEndpoints();
		}
	}
}
