using CSharpApp.Core.Dtos;
using Newtonsoft.Json;
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

/*
 * ------------------------------------
 * Endpoints for TodoService
 * ------------------------------------
 */

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
				return convertHttpRequestExceptionToResult(ex);
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
				return convertHttpRequestExceptionToResult(ex);
			}
		})
		.WithName("GetTodoById")
		.WithOpenApi()
		.Produces<TodoRecord?>(StatusCodes.Status200OK)
		.Produces(StatusCodes.Status404NotFound)
		.Produces(StatusCodes.Status503ServiceUnavailable);

/*
 * ------------------------------------
 * Endpoints for PostsService
 * ------------------------------------
 */

app.MapGet("/posts", async ([FromServices] IPostsService postsService) =>
		{
			ReadOnlyCollection<PostRecord> posts;
			try
			{
				posts = await postsService.GetAllPostsAsync();
				return Results.Ok(posts);
			}
			catch (HttpRequestException ex)
			{
				return convertHttpRequestExceptionToResult(ex);
			}
		})
		.WithName("GetPosts")
		.WithOpenApi()
		.Produces<ReadOnlyCollection<PostRecord>>(StatusCodes.Status200OK)
		.Produces(StatusCodes.Status503ServiceUnavailable);

app.MapGet("/posts/{id}", async ([FromRoute] int id, [FromServices] IPostsService postsService) =>
		{
			PostRecord? posts;
			try
			{
				posts = await postsService.GetPostByIdAsync(id);
				return Results.Ok(posts);
			}
			catch (HttpRequestException ex)
			{
				return convertHttpRequestExceptionToResult(ex);
			}
		})
		.WithName("GetPostById")
		.WithOpenApi()
		.Produces<PostRecord?>(StatusCodes.Status200OK)
		.Produces(StatusCodes.Status503ServiceUnavailable);

app.MapPost("/posts", async ([FromBody] PostRecordToAdd newPost, [FromServices] IPostsService postsService) => 
		{
			PostRecord? post;
			try
			{
				post = await postsService.AddPostAsync(newPost);
				return Results.Ok(post);
			}
			catch (HttpRequestException ex)
			{
				return convertHttpRequestExceptionToResult(ex);
			}
			catch (JsonReaderException)
			{
				return Results.StatusCode(StatusCodes.Status503ServiceUnavailable);
			}
		})
		.WithName("AddNewPost")
		.WithOpenApi()
		.Produces<PostRecord?>(StatusCodes.Status200OK)
		.Produces(StatusCodes.Status503ServiceUnavailable);

app.MapDelete("/posts/{id}", async ([FromRoute] int id, [FromServices] IPostsService postsService) =>
		{
			//TODO: Handle "Not Found"
			var deleted = await postsService.DeletePostAsync(id);
			if (deleted) return Results.Ok();
			return Results.StatusCode(StatusCodes.Status503ServiceUnavailable);
		})
		.WithName("DeletePost")
		.WithOpenApi()
		.Produces(StatusCodes.Status200OK)
		.Produces(StatusCodes.Status503ServiceUnavailable);

app.Run();