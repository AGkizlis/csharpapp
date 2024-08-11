namespace CSharpApp.Application.Services;

public sealed class TodoService(HttpClient httpClient, ILogger<TodoService> logger)
	: GenericService<TodoRecord, TodoRecordToAdd, TodoService>(httpClient, logger, "todos"), ITodoService
{
}