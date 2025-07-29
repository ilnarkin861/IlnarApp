using IlnarApp.Domain;
using IlnarApp.Domain.Tag;
using IlnarApp.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace IlnarApp.Application.Repositories;


public class TagRepository(ApplicationDbContext context) : ITagRepository
{
	private DbSet<Tag> GetDbSet() => context.Set<Tag>();
	
	public Task<Tag> InsertAsync(Tag entity)
	{
		throw new NotImplementedException();
	}

	public Task<Tag?> GetAsync(Guid id, IEntityFilter? filter)
	{
		throw new NotImplementedException();
	}

	public Task<List<Tag>> GetListAsync(int offset, int limit, IEntityFilter? filter)
	{
		throw new NotImplementedException();
	}

	public Task<Tag> UpdateAsync(Tag entity)
	{
		throw new NotImplementedException();
	}

	public Task<Tag> UpdateAsync(IEnumerable<Tag> entities)
	{
		throw new NotImplementedException();
	}

	public Task<bool> DeleteAsync(Tag entity)
	{
		throw new NotImplementedException();
	}

	public Task<bool> DeleteAsync(IEnumerable<Tag> entities)
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