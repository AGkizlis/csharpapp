namespace CSharpApp.Core.Interfaces;

public interface IGenericService<T, TWithoutId>
	where TWithoutId : class
	where T : TWithoutId, IEntity
{
	Task<ReadOnlyCollection<T>> GetAllEntitiesAsync();
	Task<T?> GetEntityByIdAsync(int id);
	Task<T?> AddEntityAsync(TWithoutId newEntity);
	Task<T?> UpdateEntityAsync(T updatedEntity);
	Task DeleteEntityAsync(int id);
}