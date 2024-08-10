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
		return response!.AsReadOnly();
	}

	public void Dispose() => httpClient?.Dispose();
}