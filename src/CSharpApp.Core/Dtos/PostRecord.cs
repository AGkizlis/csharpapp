using CSharpApp.Core.Interfaces;

namespace CSharpApp.Core.Dtos;

public class PostRecord : PostRecordToAdd, IEntity
{
	[property: JsonProperty("id")]
	public int Id {  get; set; }
}