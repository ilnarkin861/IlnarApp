using IlnarApp.Domain;
using IlnarApp.Domain.Note;
using IlnarApp.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace IlnarApp.Application.Repositories;


public class NoteRepository(ApplicationDbContext context) : INoteRepository
{
	private DbSet<Note> GetDbSet() => context.Set<Note>();
	
	public Task<Note> InsertAsync(Note entity)
	{
		throw new NotImplementedException();
	}

	public Task<Note?> GetAsync(Guid id, IEntityFilter? entityFilter)
	{
		throw new NotImplementedException();
	}

	public Task<List<Note>> GetListAsync(int offset, int limit, IEntityFilter? entityFilter)
	{
		throw new NotImplementedException();
	}

	public Task<Note> UpdateAsync(Note entity)
	{
		throw new NotImplementedException();
	}

	public Task<Note> UpdateAsync(IEnumerable<Note> entities)
	{
		throw new NotImplementedException();
	}

	public Task<bool> DeleteAsync(Note entity)
	{
		throw new NotImplementedException();
	}

	public Task<bool> DeleteAsync(IEnumerable<Note> entities)
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

	public Task<int> GetEntitiesCountAsync(int offset, int limit, IEntityFilter? entityFilter)
	{
		throw new NotImplementedException();
	}
}