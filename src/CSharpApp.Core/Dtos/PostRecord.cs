namespace CSharpApp.Core.Dtos;

public record PostRecord(
		[property: JsonProperty("userId")] int UserId,
		[property: JsonProperty("id")] int Id,
		[property: JsonProperty("title")] string Title,
		[property: JsonProperty("body")] string Body
);