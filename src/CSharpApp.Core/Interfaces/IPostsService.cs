namespace CSharpApp.Core.Interfaces;

public interface IPostsService
{
	Task<ReadOnlyCollection<PostRecord>> GetAllPostsAsync();
	Task<PostRecord?> GetPostByIdAsync(int id);
	Task<PostRecord?> AddPostAsync(PostRecordToAdd newPost);
	Task DeletePostAsync(int id);
}