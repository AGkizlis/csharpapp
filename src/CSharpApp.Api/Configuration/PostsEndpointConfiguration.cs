using CSharpApp.Core.Dtos;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace CSharpApp.Api.Configuration
{
	public static class PostsEndpointConfiguration
	{
		public static void ConfigurePostsEndpoints(this WebApplication app)
		{
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
					return ex.ConvertToResult();
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
					return ex.ConvertToResult();
				}
			})
					.WithName("GetPostById")
					.WithOpenApi()
					.Produces<PostRecord?>(StatusCodes.Status200OK)
					.Produces(StatusCodes.Status404NotFound)
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
					return ex.ConvertToResult();
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
				try
				{
					await postsService.DeletePostAsync(id);
				}
				catch (HttpRequestException ex)
				{
					return ex.ConvertToResult();
				}
				return Results.Ok();
			})
					.WithName("DeletePost")
					.WithOpenApi()
					.Produces(StatusCodes.Status200OK)
					.Produces(StatusCodes.Status404NotFound)
					.Produces(StatusCodes.Status503ServiceUnavailable);

		}
	}
}
