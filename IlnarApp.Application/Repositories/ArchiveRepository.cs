using IlnarApp.Domain;
using IlnarApp.Domain.Archive;
using IlnarApp.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace IlnarApp.Application.Repositories;


public class ArchiveRepository(ApplicationDbContext context) : IArchiveRepository
{
	private DbSet<Archive> GetDbSet => context.Set<Archive>();
	
	public Task<Archive> InsertAsync(Archive entity)
	{
		throw new NotImplementedException();
	}

	public Task<Archive?> GetAsync(Guid id, IEntityFilter? entityFilter)
	{
		throw new NotImplementedException();
	}

	public Task<List<Archive>> GetListAsync(int offset, int limit, IEntityFilter? entityFilter)
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

	public Task<bool> HasPreviousEntities(int offset, int limit, IEntityFilter? entityFilter)
	{
		throw new NotImplementedException();
	}

	public Task<bool> HasNextEntities(int offset, int limit, IEntityFilter? entityFilter)
	{
		throw new NotImplementedException();
	}

	public Task<int> GetEntitiesCountAsync(IEntityFilter? entityFilter)
	{
		throw new NotImplementedException();
	}
}