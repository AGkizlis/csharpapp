namespace CSharpApp.Core.Dtos;

public class PostRecordToAdd
{
	[property: JsonProperty("userId")]
	public int UserId { get; set; }

	[property: JsonProperty("title")]
	public string Title { get; set; } = string.Empty;

	[property: JsonProperty("body")]
	public string Body { get; set; } = string.Empty;
}