namespace IlnarApp.Domain;


public interface IRepository<TEntity> 
	where TEntity : Entity
{
	Task<TEntity> InsertAsync(TEntity entity);
	Task<TEntity?> GetAsync(Guid id,  IEntityFilter? filter);
	Task<List<TEntity>> GetListAsync(int offset, int limit, IEntityFilter? filter);
	Task<TEntity> UpdateAsync(TEntity entity);
	Task<TEntity> UpdateAsync(IEnumerable<TEntity> entities);
	Task<bool> DeleteAsync(TEntity entity);
	Task<bool> DeleteAsync(IEnumerable<TEntity> entities);
	Task<bool> HasPreviousEntitiesAsync(int offset, int limit, IEntityFilter? filter);
	Task<bool> HasNextEntities(int offset, int limit, IEntityFilter? filter);
	Task<int> GetEntitiesCount(int offset, int limit, IEntityFilter? filter);
}