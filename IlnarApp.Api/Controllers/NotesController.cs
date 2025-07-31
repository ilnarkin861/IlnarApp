using IlnarApp.Api.Actions;
using IlnarApp.Api.Exceptions;
using IlnarApp.Application.Helpers;
using IlnarApp.Application.Models;
using IlnarApp.Domain.Note;
using Microsoft.AspNetCore.Mvc;

namespace IlnarApp.Api.Controllers;


public class NotesController(INoteRepository noteRepository) : BaseController
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
		[FromQuery] NoteFilterRequest filter)
	{
		var notesLimit = limit is 0 or > 10 ? 10 : limit;

		var notes = await noteRepository.GetListAsync(offset, notesLimit, filter);

		var notesCount = await noteRepository.GetEntitiesCountAsync(filter);
		
		var response = new PaginationResponse
		{
			Data = notes,
			Pagination = new Paginator(notesCount, offset, notesLimit)
		};
		
		return Ok(response);
	}


	[HttpPost]
	[Route("add")]
	[ValidationAction]
	public async Task<IActionResult> InsertAsync([FromBody] NoteDto noteDto)
	{
		var note = new Note
		{
			Title = noteDto.Title,
			Text = noteDto.Text,
			Date = noteDto.Date,
			NoteType = noteDto.NoteType,
			Archive = noteDto.Archive,
			Tags = noteDto.Tags ?? []
		};

		return Ok(await noteRepository.InsertAsync(note));
	}


	[HttpPut]
	[Route("edit/{id:guid}")]
	[ValidationAction]
	public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] NoteDto noteDto)
	{
		var note = await GetById(id);

		if (note == null)
		{
			throw new EntityNotFoundException("Запись не найдена");
		}

		note.Title = noteDto.Title;
		note.Text = noteDto.Text;
		note.Date = noteDto.Date;
		note.NoteType = noteDto.NoteType;
		note.Archive = noteDto.Archive;
		note.Tags = noteDto.Tags ?? [];
		
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

		var response = new DefaultResponse
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