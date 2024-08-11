namespace CSharpApp.Application.Services
{
	public abstract class GenericService<T, TWithoutId, K>(
		HttpClient httpClient,
		ILogger<K> logger,
		string endpoint)
		: IGenericService<T, TWithoutId>, IDisposable
		where TWithoutId : class
		where T : class, TWithoutId, IEntity
		where K : GenericService<T, TWithoutId, K>
	{
		private bool disposed = false;
		private bool httpClientIsDisposed = false;

		public async Task<ReadOnlyCollection<T>> GetAllEntitiesAsync()
		{
			var response = await GetFromJsonAsync<List<T>>(endpoint);
			response ??= [];
			return response.AsReadOnly();
		}

		public async Task<T?> GetEntityByIdAsync(int id)
		{
			return await GetFromJsonAsync<T>(endpoint + $"/{id}");
		}

		public async Task<T?> AddEntityAsync(TWithoutId newEntity)
		{
			var response = await httpClient.PostAsJsonAsync(endpoint, newEntity);
			response.EnsureSuccessStatusCode();
			T? result;
			try
			{
				result = await response.Content.ReadFromJsonAsync<T>();
			}
			catch (Exception)
			{
				throw new JsonReaderException();
			}
			return result;
		}

		public async Task<T?> UpdateEntityAsync(T updatedEntity)
		{
			var response = await httpClient.PutAsJsonAsync(endpoint + $"/{updatedEntity.Id}", updatedEntity);
			response.EnsureSuccessStatusCode();
			T? result;
			try
			{
				result = await response.Content.ReadFromJsonAsync<T>();
			}
			catch (Exception)
			{
				throw new JsonReaderException();
			}
			return result;
		}

		public async Task DeleteEntityAsync(int id)
		{
			var response = await httpClient.DeleteAsync(endpoint + $"/{id}");
			response.EnsureSuccessStatusCode();
		}

		public async Task DeleteEntityAsync(T entityToDelete)
		{
			var response = await httpClient.DeleteAsync(endpoint + $"/{entityToDelete.Id}");
			response.EnsureSuccessStatusCode();
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposed)
			{
				return;
			}

			if (disposing)
			{
				// Dispose managed state (managed objects).
				if (!httpClientIsDisposed)
				{
					httpClient.Dispose();
					httpClientIsDisposed = true;
				}
			}

			disposed = true;
		}

		private async Task<M?> GetFromJsonAsync<M>(string? requestUri) where M : class
		{
			M? response;
			try
			{
				response = await httpClient.GetFromJsonAsync<M>(requestUri);
			}
			catch (HttpRequestException ex)
			{
				switch (ex.StatusCode)
				{
					case System.Net.HttpStatusCode.NoContent:
						return default;
					default:
						logger.LogWarning(ex, "Third party API for Posts failed in PostsService.");
						throw;
				}
			}
			return response;
		}

	}
}
