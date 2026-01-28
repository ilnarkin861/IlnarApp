using IlnarApp.Domain;
using IlnarApp.Domain.Tag;
using IlnarApp.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace IlnarApp.Application.Repositories;


public class TagRepository(ApplicationDbContext context) : ITagRepository
{
	private DbSet<Tag> GetDbSet() => context.Set<Tag>();
	
	
	public async Task<Tag> InsertAsync(Tag tag)
	{
		var entity = await GetDbSet().AddAsync(tag);
		await context.SaveChangesAsync();
		return entity.Entity;
	}

	
	public async Task<Tag?> GetAsync(Guid id, IEntityFilter? entityFilter)
	{
		return await GetDbSet().FirstOrDefaultAsync(t => t.Id == id && t.Deleted == false);
	}
	

	public async Task<List<Tag>> GetListAsync(int offset, int limit, IEntityFilter? entityFilter)
	{
		return await GetDbSet().OrderByDescending(t => t.CreatedAt)
			.Skip(offset)
			.Take(limit)
			.Where(t => t.Deleted == false)
			.ToListAsync();
	}
	

	public async Task<Tag> UpdateAsync(Tag tag)
	{
		var entity = GetDbSet().Update(tag);
		await context.SaveChangesAsync();
		return entity.Entity;
	}
	

	public Task<Tag> UpdateAsync(IEnumerable<Tag> entities)
	{
		throw new NotImplementedException();
	}

	
	public async Task<bool> DeleteAsync(Tag entity)
	{
		context.Update(entity);
		return await context.SaveChangesAsync() > 0;
	}
	

	public Task<bool> DeleteAsync(IEnumerable<Tag> entities)
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
		return await GetDbSet().Where(e => e.Deleted == false).CountAsync();
	}
}