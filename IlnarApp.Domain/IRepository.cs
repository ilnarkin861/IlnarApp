

namespace IlnarApp.Domain;

public interface IRepository<TEntity> 
	where TEntity : Entity
{
	Task<TEntity> InsertAsync(TEntity entity);
	Task<TEntity?> GetAsync(Guid id,  IEntityFilter? filter);
	Task<List<TEntity>> GetListAsync(int offset, int limit, IEntityFilter? filter);
	Task UpdateAsync(TEntity entity);
	Task UpdateAsync(IEnumerable<TEntity> entities);
	Task Delete(TEntity entity);
	Task DeleteRangeAsync(IEnumerable<TEntity> entities);
}