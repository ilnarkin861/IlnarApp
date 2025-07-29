using IlnarApp.Domain;
using IlnarApp.Domain.Archive;
using Microsoft.EntityFrameworkCore;


namespace IlnarApp.Infrastructure.Repositories;


public class ArchiveRepository(ApplicationDbContext context) : IArchiveRepository
{
	private DbSet<Archive> GetDbSet => context.Set<Archive>();
	
	public Task<Archive> InsertAsync(Archive entity)
	{
		throw new NotImplementedException();
	}

	public Task<Archive?> GetAsync(Guid id, IEntityFilter? filter)
	{
		throw new NotImplementedException();
	}

	public Task<List<Archive>> GetListAsync(int offset, int limit, IEntityFilter? filter)
	{
		throw new NotImplementedException();
	}

	public Task<Archive> UpdateAsync(Archive entity)
	{
		throw new NotImplementedException();
	}

	public Task<Archive> UpdateAsync(IEnumerable<Archive> entities)
	{
		throw new NotImplementedException();
	}

	public Task<bool> DeleteAsync(Archive entity)
	{
		throw new NotImplementedException();
	}

	public Task<bool> DeleteAsync(IEnumerable<Archive> entities)
	{
		throw new NotImplementedException();
	}

	public Task<bool> HasPreviousEntities(int offset, int limit, IEntityFilter? filter)
	{
		throw new NotImplementedException();
	}

	public Task<bool> HasNextEntities(int offset, int limit, IEntityFilter? filter)
	{
		throw new NotImplementedException();
	}

	public Task<int> GetEntitiesCountAsync(int offset, int limit, IEntityFilter? filter)
	{
		throw new NotImplementedException();
	}
}