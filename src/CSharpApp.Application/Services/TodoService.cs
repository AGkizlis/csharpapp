namespace CSharpApp.Application.Services;

public sealed class TodoService(HttpClient httpClient) : ITodoService, IDisposable
{

	public async Task<TodoRecord?> GetTodoById(int id)
	{
		return await httpClient.GetFromJsonAsync<TodoRecord>($"todos/{id}");
	}

	public async Task<ReadOnlyCollection<TodoRecord>> GetAllTodos()
	{
		var response = await httpClient.GetFromJsonAsync<List<TodoRecord>>($"todos");
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
					throw;
			}
		}
		return response;
	}
}