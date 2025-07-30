namespace IlnarApp.Domain;


public interface IRepository<TEntity> 
	where TEntity : Entity
{
	Task<TEntity> InsertAsync(TEntity entity);
	Task<TEntity?> GetAsync(Guid id,  IEntityFilter? entityFilter);
	Task<List<TEntity>> GetListAsync(int offset, int limit, IEntityFilter? entityFilter);
	Task<TEntity> UpdateAsync(TEntity entity);
	Task<TEntity> UpdateAsync(IEnumerable<TEntity> entities);
	Task<bool> DeleteAsync(TEntity entity);
	Task<bool> DeleteAsync(IEnumerable<TEntity> entities);
	Task<bool> HasPreviousEntities(int offset, int limit, IEntityFilter? entityFilter);
	Task<bool> HasNextEntities(int offset, int limit, IEntityFilter? entityFilter);
	Task<int> GetEntitiesCountAsync(IEntityFilter? entityFilter);
}