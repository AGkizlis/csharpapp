using CSharpApp.Core.Dtos;
using System.Collections.ObjectModel;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger());

builder.Configuration.AddEnvironmentVariables();

// Add services to the container.
builder.Services.AddDefaultConfiguration(builder.Configuration);

// Configure Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

Func<HttpRequestException, IResult> convertHttpRequestExceptionToResult = (HttpRequestException ex) =>
{
	return ex.StatusCode switch
	{
		System.Net.HttpStatusCode.NotFound => Results.NotFound(),
		System.Net.HttpStatusCode.ServiceUnavailable => Results.StatusCode(StatusCodes.Status503ServiceUnavailable),
		_ => Results.NoContent(),
	};
};

app.MapGet("/todos", async ([FromServices] ITodoService todoService) =>
		{
			ReadOnlyCollection<TodoRecord> todos;
			try
			{
				todos = await todoService.GetAllTodos();
				return Results.Ok(todos);
			}
			catch (HttpRequestException ex)
			{
				return convertHttpRequestExceptionToResult(ex);
			}
		})
		.WithName("GetTodos")
		.WithOpenApi()
		.Produces<ReadOnlyCollection<TodoRecord>>(StatusCodes.Status200OK);

app.MapGet("/todos/{id}", async ([FromRoute] int id, [FromServices] ITodoService todoService) =>
		{
			TodoRecord? todos;
			try
			{
				todos = await todoService.GetTodoById(id);
				return Results.Ok(todos);
			}
			catch (HttpRequestException ex)
			{
				return convertHttpRequestExceptionToResult(ex);
			}
		})
		.WithName("GetTodosById")
		.WithOpenApi()
		.Produces<TodoRecord?>(StatusCodes.Status200OK)
		.Produces(StatusCodes.Status404NotFound);

app.Run();