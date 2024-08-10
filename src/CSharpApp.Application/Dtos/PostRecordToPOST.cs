namespace CSharpApp.Application.Dtos;

public record PostRecordToPOST(
		[property: JsonProperty("userId")] int UserId,
		[property: JsonProperty("title")] string Title,
		[property: JsonProperty("body")] string Body
);