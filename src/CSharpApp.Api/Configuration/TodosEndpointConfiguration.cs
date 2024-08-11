using CSharpApp.Core.Dtos;
using System.Collections.ObjectModel;

namespace CSharpApp.Api.Configuration
{
	public static class TodosEndpointConfiguration
	{
		public static void ConfigureTodosEndpoints(this WebApplication app)
		{
			app.MapGet("/todos", async ([FromServices] ITodoService todoService) =>
			{
				ReadOnlyCollection<TodoRecord> todos;
				try
				{
					todos = await todoService.GetAllTodosAsync();
					return Results.Ok(todos);
				}
				catch (HttpRequestException ex)
				{
					return ex.ConvertToResult();
				}
			})
		.WithName("GetTodos")
		.WithOpenApi()
		.Produces<ReadOnlyCollection<TodoRecord>>(StatusCodes.Status200OK)
		.Produces(StatusCodes.Status503ServiceUnavailable);

			app.MapGet("/todos/{id}", async ([FromRoute] int id, [FromServices] ITodoService todoService) =>
			{
				TodoRecord? todo;
				try
				{
					todo = await todoService.GetTodoByIdAsync(id);
					return Results.Ok(todo);
				}
				catch (HttpRequestException ex)
				{
					return ex.ConvertToResult();
				}
			})
					.WithName("GetTodoById")
					.WithOpenApi()
					.Produces<TodoRecord?>(StatusCodes.Status200OK)
					.Produces(StatusCodes.Status404NotFound)
					.Produces(StatusCodes.Status503ServiceUnavailable);

		}
	}
}
