using IlnarApp.Application.Models;
using IlnarApp.Domain;
using IlnarApp.Domain.Note;
using IlnarApp.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace IlnarApp.Application.Repositories;


public class NoteRepository(ApplicationDbContext context) : INoteRepository
{
	private DbSet<Note> GetDbSet() => context.Set<Note>();
	
	public async Task<Note> InsertAsync(Note note)
	{
		var entity = await GetDbSet().AddAsync(note);
		context.Entry(entity.Entity.NoteType).State = EntityState.Unchanged;

		if (entity.Entity.Archive != null)
		{
			context.Entry(entity.Entity.Archive).State = EntityState.Unchanged;
		}
		
		await context.SaveChangesAsync();
		return entity.Entity;
	}

	
	public async Task<Note?> GetAsync(Guid id, IEntityFilter? entityFilter)
	{
		return await GetDbSet()
			.Include(x => x.NoteType)
			.Include(x => x.Archive)
			.FirstOrDefaultAsync(x => x.Id == id);
	}


	
	public async Task<List<Note>> GetListAsync(int offset, int limit, IEntityFilter? entityFilter)
	{
		if (entityFilter == null)
		{
			return await GetDbSet()
				.Skip(offset)
				.Take(limit)
				.Include(x => x.NoteType)
				.Include(x => x.Archive)
				.ToListAsync();
		}

		return await BuildQuery(entityFilter)
			.Skip(offset)
			.Take(limit)
			.Include(x => x.NoteType)
			.Include(x => x.Archive)
			.OrderByDescending(x => x.Date)
			.ToListAsync();
	}

	
	public async Task<Note> UpdateAsync(Note note)
	{
		var entity = GetDbSet().Update(note);
		context.Entry(entity.Entity.NoteType).State = EntityState.Unchanged;

		if (entity.Entity.Archive != null)
		{
			context.Entry(entity.Entity.Archive).State = EntityState.Unchanged;
		}
		
		await context.SaveChangesAsync();
		return entity.Entity;
	}

	public Task<Note> UpdateAsync(IEnumerable<Note> entities)
	{
		throw new NotImplementedException();
	}

	public async Task<bool> DeleteAsync(Note entity)
	{
		context.Remove(entity);
		return await context.SaveChangesAsync() > 0;
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

	public async Task<int> GetEntitiesCountAsync(IEntityFilter? entityFilter)
	{
		if (entityFilter == null) return await GetDbSet().CountAsync();

		return await BuildQuery(entityFilter).CountAsync();
	}


	public async Task<List<Note>> GetEntitiesAddedYearAgo(int offset, int limit, int year, int month, int day)
	{
		return await GetDbSet()
			.Skip(offset)
			.Take(limit)
			.Where(x => x.Date.Year == year && x.Date.Month == month && x.Date.Day == day)
			.ToListAsync();
	}
	
	private IQueryable<Note> BuildQuery(IEntityFilter entityFilter)
	{
		var query = GetDbSet().AsQueryable();
	
		var filter = (NoteFilterRequest)entityFilter;
		
		if (filter.NoteTypeId != null)
		{
			query = query.Where(x => x.NoteType.Id == filter.NoteTypeId);
		}

		if (filter.ArchiveId != null)
		{
			query = query.Where(x => x.Archive != null && x.Archive.Id == filter.ArchiveId);
		}

		if (filter.Year != null)
		{
			query = query.Where(x => x.Date.Year == filter.Year);
		}
			
		if (filter.Month != null)
		{
			query = query.Where(x => x.Date.Month == filter.Month);
		}

		if (filter.TagIds is { Count: > 0 })
		{
			query = query.Where(x => filter.TagIds.Contains(x.Id));
		}

		return query;
	}
}