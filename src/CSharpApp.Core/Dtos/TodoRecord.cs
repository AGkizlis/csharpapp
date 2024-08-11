using CSharpApp.Core.Interfaces;

namespace CSharpApp.Core.Dtos;

public class TodoRecord : TodoRecordToAdd, IEntity
{
	[property: JsonProperty("id")]
	public int Id { get; set; }
}