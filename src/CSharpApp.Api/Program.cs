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

//app.UseHttpsRedirection();

app.MapGet("/todos", async ([FromServices] ITodoService todoService) =>
		{
			var todos = await todoService.GetAllTodos();
			return todos;
		})
		.WithName("GetTodos")
		.WithOpenApi();

app.MapGet("/todos/{id}", async ([FromRoute] int id, [FromServices] ITodoService todoService) =>
		{
			var todos = await todoService.GetTodoById(id);
			return todos;
		})
		.WithName("GetTodosById")
		.WithOpenApi();

app.Run();