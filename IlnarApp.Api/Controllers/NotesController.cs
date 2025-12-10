using IlnarApp.Api.Actions;
using IlnarApp.Application.Exceptions;
using IlnarApp.Application.Helpers;
using IlnarApp.Application.Models;
using IlnarApp.Domain.Archive;
using IlnarApp.Domain.Note;
using IlnarApp.Domain.Tag;
using Microsoft.AspNetCore.Mvc;

namespace IlnarApp.Api.Controllers;


public class NotesController(
	INoteRepository noteRepository,
	INoteTypeRepository noteTypeRepository,
	IArchiveRepository archiveRepository,
	ITagRepository tagRepository) : BaseController
{
	[HttpGet]
	[Route("{id:guid}")]
	public async Task<IActionResult> GetAsync(Guid id)
	{
		var note = await GetById(id);

		if (note == null)
		{
			throw new EntityNotFoundException("Запись не найдена");
		}
		
		return Ok(note);
	}


	[HttpGet]
	[Route("")]
	public async Task<IActionResult> GetListAsync([FromQuery] int offset, [FromQuery] int limit, 
		[FromQuery] NoteFilterData filter)
	{
		var notesLimit = limit is 0 or > 10 ? 10 : limit;

		var notes = await noteRepository.GetListAsync(offset, notesLimit, filter);

		var notesCount = await noteRepository.GetEntitiesCountAsync(filter);
		
		var response = new PaginationData
		{
			Data = notes,
			Pagination = new Paginator(notesCount, offset, notesLimit)
		};
		
		return Ok(response);
	}


	[HttpPost]
	[Route("add")]
	[ValidationAction]
	public async Task<IActionResult> InsertAsync([FromBody] NoteData noteData)
	{
		var noteType = await noteTypeRepository.GetAsync(noteData.NoteType.Id, null);

		if (noteType == null) throw new EntityNotFoundException("Тип записи не найден");

		var note = new Note
		{
			Title = noteData.Title,
			Text = noteData.Text,
			NoteType = noteType,
			Date = noteData.Date
		};
		
		if (noteData.Archive != null)
		{
			var archive = await archiveRepository.GetAsync(noteData.Archive.Id, null);

			if (archive != null) note.Archive = archive;
		}
		
		if (noteData.Tags is not { Count: > 0 }) return Ok(await noteRepository.InsertAsync(note));
		
		var tags = new List<Tag>();

		foreach (var item in noteData.Tags)
		{
			var tag = await tagRepository.GetAsync(item.Id, null);
			if (tag != null) tags.Add(tag);
		}

		note.Tags = tags;

		return Ok(await noteRepository.InsertAsync(note));
	}


	[HttpPut]
	[Route("edit/{id:guid}")]
	[ValidationAction]
	public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] NoteData noteData)
	{
		var note = await GetById(id);

		if (note == null)
		{
			throw new EntityNotFoundException("Запись не найдена");
		}
		
		var noteType = await noteTypeRepository.GetAsync(noteData.NoteType.Id, null);

		if (noteType == null) throw new EntityNotFoundException("Тип записи не найден");
		
		note.Title = noteData.Title;
		note.Text = noteData.Text;
		note.Date = noteData.Date;
		note.NoteType = noteType;
		note.Archive = noteData.Archive;
		
		if (noteData.Archive != null)
		{
			var archive = await archiveRepository.GetAsync(noteData.Archive.Id, null);

			if (archive != null) note.Archive = archive;
		}
		
		note.Tags?.Clear();
		
		if (noteData.Tags is not { Count: > 0 }) return Ok(await noteRepository.UpdateAsync(note));
		
		var tags = new List<Tag>();
		
		foreach (var item in noteData.Tags)
		{
			var tag = await tagRepository.GetAsync(item.Id, null);
			if (tag != null) tags.Add(tag);
		}

		note.Tags = tags;
		
		return Ok(await noteRepository.UpdateAsync(note));
	}
	
	
	[HttpDelete]
	[Route("delete/{id:guid}")]
	public async Task<IActionResult> DeleteAsync(Guid id)
	{
		var note = await GetById(id);

		if (note == null)
		{
			throw new EntityNotFoundException("Запись не найдена");
		}

		var result = await noteRepository.DeleteAsync(note);

		var message = result ? "Запись успешно удалена" : "Ошибка при удалении записи";

		var errorsList = new List<string> { message };

		var response = new ResponseData
		{
			Messages = errorsList,
			Success = result
		};

		return result ? Ok(response) : BadRequest(response);
	}
	
	
	private async Task<Note?> GetById(Guid id)
	{
		return await noteRepository.GetAsync(id, null);
	}
	
}