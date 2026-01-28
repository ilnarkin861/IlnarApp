using IlnarApp.Domain;
using IlnarApp.Domain.Note;
using IlnarApp.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace IlnarApp.Application.Repositories;


public class NoteTypeRepository(ApplicationDbContext context) : INoteTypeRepository
{
	private DbSet<NoteType> GetDbSet() => context.Set<NoteType>();
	
	
	public async Task<NoteType> InsertAsync(NoteType noteType)
	{
		var entity = await GetDbSet().AddAsync(noteType);
		await context.SaveChangesAsync();
		return entity.Entity;
	}
	

	public async Task<NoteType?> GetAsync(Guid id, IEntityFilter? entityFilter)
	{
		return await GetDbSet().FirstOrDefaultAsync(nt => nt.Id == id && nt.Deleted == false);
	}

	
	public async Task<List<NoteType>> GetListAsync(int offset, int limit, IEntityFilter? entityFilter)
	{
		return await GetDbSet().Where(nt => nt.Deleted == false).ToListAsync();
	}

	
	public async Task<NoteType> UpdateAsync(NoteType noteType)
	{
		var entity = GetDbSet().Update(noteType);
		await context.SaveChangesAsync();
		return entity.Entity;
	}
	

	public Task<NoteType> UpdateAsync(IEnumerable<NoteType> entities)
	{
		throw new NotImplementedException();
	}

	
	public async Task<bool> DeleteAsync(NoteType entity)
	{
		context.Update(entity);
		return await context.SaveChangesAsync() > 0;
	}
	

	public Task<bool> DeleteAsync(IEnumerable<NoteType> entities)
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