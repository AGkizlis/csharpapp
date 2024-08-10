namespace CSharpApp.Application.Services;

public sealed class TodoService(HttpClient httpClient, ILogger<TodoService> logger) : ITodoService, IDisposable
{

	public async Task<TodoRecord?> GetTodoByIdAsync(int id)
	{
		return await GetFromJsonAsync<TodoRecord>($"todos/{id}");
	}

	public async Task<ReadOnlyCollection<TodoRecord>> GetAllTodosAsync()
	{
		var response = await GetFromJsonAsync<List<TodoRecord>>($"todos");
		response ??= [];
		return response.AsReadOnly();
	}

	public void Dispose() => httpClient?.Dispose();

	private async Task<T?> GetFromJsonAsync<T>(string? requestUri) where T: class
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
					logger.LogWarning(ex, "Third party API for Todos failed in TodoService.");
					throw;
			}
		}
		return response;
	}
}