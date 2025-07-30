using IlnarApp.Domain;
using IlnarApp.Domain.Archive;
using IlnarApp.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace IlnarApp.Application.Repositories;


public class ArchiveRepository(ApplicationDbContext context) : IArchiveRepository
{
	private DbSet<Archive> GetDbSet() => context.Set<Archive>();
	
	public async Task<Archive> InsertAsync(Archive archive)
	{
		var entity = await GetDbSet().AddAsync(archive);
		await context.SaveChangesAsync();
		return entity.Entity;
	}

	public async Task<Archive?> GetAsync(Guid id, IEntityFilter? entityFilter)
	{
		return await GetDbSet().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
	}

	public async Task<List<Archive>> GetListAsync(int offset, int limit, IEntityFilter? entityFilter)
	{
		return await GetDbSet().OrderByDescending(x => x.CreatedAt)
			.Skip(offset)
			.Take(limit)
			.IgnoreAutoIncludes()
			.ToListAsync();
	}

	public async Task<Archive> UpdateAsync(Archive archive)
	{
		var entity = GetDbSet().Update(archive);
		await context.SaveChangesAsync();
		return entity.Entity;
	}

	public Task<Archive> UpdateAsync(IEnumerable<Archive> entities)
	{
		throw new NotImplementedException();
	}

	public async Task<bool> DeleteAsync(Archive entity)
	{
		context.Remove(entity);
		return await context.SaveChangesAsync() > 0;
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

	public async Task<int> GetEntitiesCountAsync(IEntityFilter? entityFilter)
	{
		return await GetDbSet().CountAsync();
	}
}