namespace CSharpApp.Core.Dtos;

public class TodoRecordToAdd
{
	[property: JsonProperty("userId")]
	public int UserId { get; set; }

	[property: JsonProperty("title")]
	public string Title { get; set; } = string.Empty;

	[property: JsonProperty("completed")]
	public bool Completed { get; set; }
}
