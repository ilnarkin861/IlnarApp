using IlnarApp.Api.Actions;
using IlnarApp.Application.Exceptions;
using IlnarApp.Application.Helpers;
using IlnarApp.Application.Models;
using IlnarApp.Application.Services.S3;
using IlnarApp.Domain.Archive;
using IlnarApp.Domain.Note;
using IlnarApp.Domain.Tag;
using Microsoft.AspNetCore.Mvc;

namespace IlnarApp.Api.Controllers;


public class NotesController(
	INoteRepository noteRepository,
	INoteTypeRepository noteTypeRepository,
	IArchiveRepository archiveRepository,
	ITagRepository tagRepository,
	IS3Service<NoteImage> s3Service) : BaseController
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
	public async Task<IActionResult> InsertAsync([FromBody] NoteRequest noteRequest)
	{
		var noteType = await noteTypeRepository.GetAsync(noteRequest.NoteType.Id, null);

		if (noteType == null) throw new EntityNotFoundException("Тип записи не найден");

		var note = new Note
		{
			Title = noteRequest.Title,
			Text = noteRequest.Text,
			NoteType = noteType,
			Date = noteRequest.Date
		};
		
		if (noteRequest.Archive != null)
		{
			var archive = await archiveRepository.GetAsync(noteRequest.Archive.Id, null);

			if (archive != null) note.Archive = archive;
		}
		
		if (noteRequest.Tags is not { Count: > 0 } && noteRequest.NoteImages is not { Count: > 0 }) return Ok(await noteRepository.InsertAsync(note));

		if (noteRequest.Tags is { Count: > 0 })
		{
			var tags = new List<Tag>();

			foreach (var item in noteRequest.Tags)
			{
				var tag = await tagRepository.GetAsync(item.Id, null);
				if (tag != null) tags.Add(tag);
			}

			note.Tags = tags;
		}
		
		if (noteRequest.NoteImages is { Count: > 0 })
		{
			var images = new List<NoteImage>();
			foreach (var item in noteRequest.NoteImages)
			{
				var image = await s3Service.GetNoteImageAsync(item.Id); 
				
				if (image != null) 
				{
					images.Add(image);
				}
			}
			note.NoteImages = images;
		}




		return Ok(await noteRepository.InsertAsync(note));
	}


	[HttpPut]
	[Route("edit/{id:guid}")]
	[ValidationAction]
	public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] NoteRequest noteRequest)
	{
		var note = await GetById(id);

		if (note == null)
		{
			throw new EntityNotFoundException("Запись не найдена");
		}
		
		var noteType = await noteTypeRepository.GetAsync(noteRequest.NoteType.Id, null);

		if (noteType == null) throw new EntityNotFoundException("Тип записи не найден");
		
		note.Title = noteRequest.Title;
		note.Text = noteRequest.Text;
		note.Date = noteRequest.Date;
		note.NoteType = noteType;
		note.Archive = noteRequest.Archive;
		
		if (noteRequest.Archive != null)
		{
			var archive = await archiveRepository.GetAsync(noteRequest.Archive.Id, null);

			if (archive != null) note.Archive = archive;
		}
		
		note.Tags?.Clear();
		
		if (noteRequest.Tags is not { Count: > 0 } && noteRequest.NoteImages is not { Count: > 0 }) return Ok(await noteRepository.UpdateAsync(note));
		
		if (noteRequest.Tags is { Count: > 0 })
		{
			var tags = new List<Tag>();

			foreach (var item in noteRequest.Tags)
			{
				var tag = await tagRepository.GetAsync(item.Id, null);
				if (tag != null) tags.Add(tag);
			}

			note.Tags = tags;
		}
		
		note.NoteImages?.Clear();

		if (noteRequest.NoteImages is { Count: > 0 })
		{
			if (note.NoteImages == null) note.NoteImages = new List<NoteImage>();
			
			foreach (var item in noteRequest.NoteImages)
			{
				var image = await s3Service.GetNoteImageAsync(item.Id);
				if (image != null) note.NoteImages.Add(image);
			}
		}
		
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
		
		note.Deleted = true;
		
		note.Tags?.Clear();
		
		note.NoteImages?.Clear();

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