namespace CSharpApp.Application.Services
{
	public sealed class PostsService(HttpClient httpClient, ILogger<PostsService> logger)
		: GenericService<PostRecord, PostRecordToAdd, PostsService>(httpClient, logger, "posts"), IPostsService
	{
	}
}
