namespace CSharpApp.Core.Dtos;

public record PostRecordToAdd(
		[property: JsonProperty("userId")] int UserId,
		[property: JsonProperty("title")] string Title,
		[property: JsonProperty("body")] string Body
);