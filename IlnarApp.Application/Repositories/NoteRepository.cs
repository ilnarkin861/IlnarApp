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

		await context.SaveChangesAsync();

		return entity.Entity;
	}

	
	public async Task<Note?> GetAsync(Guid id, IEntityFilter? entityFilter)
	{
		return await GetDbSet()
			.Include(n => n.NoteType)
			.Include(n => n.Archive)
			.Include(n => n.Tags)
			.FirstOrDefaultAsync(n => n.Id == id && n.Deleted == false);
	}

	
	public async Task<List<Note>> GetListAsync(int offset, int limit, IEntityFilter? entityFilter)
	{
		return await BuildQuery(entityFilter)
			.AsNoTracking()
			.Where(n => n.Deleted == false)
			.OrderByDescending(n => n.Date)
			.ThenByDescending(n => n.CreatedDate)
			.Include(n => n.NoteType)
			.Include(n => n.Archive)
			.Include(n => n.Tags)
			.Skip(offset)
			.Take(limit)
			.AsSplitQuery()
			.ToListAsync();
	}

	
	public async Task<Note> UpdateAsync(Note note)
	{
		var entity = GetDbSet().Update(note);
		
		await context.SaveChangesAsync();
		
		return entity.Entity;
	}

	
	public Task<Note> UpdateAsync(IEnumerable<Note> entities)
	{
		throw new NotImplementedException();
	}
	

	public async Task<bool> DeleteAsync(Note entity)
	{
		context.Update(entity);
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

		return await BuildQuery(entityFilter).Where(e => e.Deleted == false).CountAsync();
	}
	
	
	private IQueryable<Note> BuildQuery(IEntityFilter? entityFilter)
	{
		var query = GetDbSet().AsQueryable();

		if (entityFilter == null) return query;
		var filter = (NoteFilterRequest)entityFilter;
		
		if (filter.NoteTypeId != null)
		{
			query = query.Where(n => n.NoteType.Id == filter.NoteTypeId);
		}

		if (filter.ArchiveId != null)
		{
			query = query.Where(n => n.Archive != null && n.Archive.Id == filter.ArchiveId);
		}

		if (filter.Year != null)
		{
			query = query.Where(n => n.Date.Year == filter.Year);
		}
			
		if (filter.Month != null)
		{
			query = query.Where(n => n.Date.Month == filter.Month);
		}
		
		if (filter is { Month: not null, Year: not null })
		{
			var startDate = new DateOnly(filter.Year.Value, filter.Month.Value, 1);
			var endDate = startDate.AddMonths(1);
    
			query = query.Where(n => n.Date >= startDate && n.Date < endDate);
		}

		if (filter.TagIds is { Count: > 0 })
		{
			query = query
				// ReSharper disable once NullableWarningSuppressionIsUsed
				.Where(n => n.Tags!.Any(t => filter.TagIds.Contains(t.Id)));
		}

		return query;
	}
}