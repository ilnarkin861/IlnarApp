using IlnarApp.Domain;
using IlnarApp.Domain.Note;
using Microsoft.EntityFrameworkCore;


namespace IlnarApp.Infrastructure.Repositories;


public class NoteTypeRepository(ApplicationDbContext context) : INoteTypeRepository
{
	private DbSet<NoteType> GetDbSet() => context.Set<NoteType>();
	
	
	public Task<NoteType> InsertAsync(NoteType entity)
	{
		throw new NotImplementedException();
	}
	

	public Task<NoteType?> GetAsync(Guid id, IEntityFilter? filter)
	{
		throw new NotImplementedException();
	}

	public Task<List<NoteType>> GetListAsync(int offset, int limit, IEntityFilter? filter)
	{
		throw new NotImplementedException();
	}

	public Task<NoteType> UpdateAsync(NoteType entity)
	{
		throw new NotImplementedException();
	}

	public Task<NoteType> UpdateAsync(IEnumerable<NoteType> entities)
	{
		throw new NotImplementedException();
	}

	public Task<bool> DeleteAsync(NoteType entity)
	{
		throw new NotImplementedException();
	}

	public Task<bool> DeleteAsync(IEnumerable<NoteType> entities)
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