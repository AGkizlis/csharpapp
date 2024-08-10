namespace CSharpApp.Application.Services
{
	public sealed class PostsService(HttpClient httpClient, ILogger<PostsService> logger) : IPostsService, IDisposable
	{
		public async Task<ReadOnlyCollection<PostRecord>> GetAllPostsAsync()
		{
			var response = await GetFromJsonAsync<List<PostRecord>>($"posts");
			response ??= [];
			return response.AsReadOnly();
		}

		public async Task<PostRecord?> GetPostByIdAsync(int id)
		{
			return await GetFromJsonAsync<PostRecord>($"posts/{id}");
		}

		public async Task<PostRecord?> AddPostAsync(PostRecordToAdd newPost)
		{
			var response = await httpClient.PostAsJsonAsync($"posts", newPost);
			response.EnsureSuccessStatusCode();
			PostRecord? result;
			try
			{
				result = await response.Content.ReadFromJsonAsync<PostRecord>();
			}
			catch (Exception)
			{
				throw new JsonReaderException();
			}
			return result;
		}

		public async Task<bool> DeletePostAsync(int id)
		{
			var response = await httpClient.DeleteAsync($"posts/{id}");
			return (response.StatusCode == System.Net.HttpStatusCode.OK);
		}

		public void Dispose() => httpClient?.Dispose();

		private async Task<T?> GetFromJsonAsync<T>(string? requestUri) where T : class
		{
			T? response;
			try
			{
				response = await httpClient.GetFromJsonAsync<T>(requestUri);
			}
			catch (HttpRequestException ex)
			{
				switch (ex.StatusCode)
				{
					case System.Net.HttpStatusCode.NoContent:
						return default;
					default:
						logger.LogWarning(ex, "Third party API for Posts failed in PostsService.");
						throw;
				}
			}
			return response;
		}

	}
}
