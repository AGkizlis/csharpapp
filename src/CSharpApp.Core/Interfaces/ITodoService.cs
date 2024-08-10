namespace CSharpApp.Core.Interfaces;

public interface ITodoService
{
	Task<TodoRecord?> GetTodoByIdAsync(int id);
	Task<ReadOnlyCollection<TodoRecord>> GetAllTodosAsync();
}