using IlnarApp.Api.Actions;
using IlnarApp.Application.Exceptions;
using IlnarApp.Application.Helpers;
using IlnarApp.Application.Models;
using IlnarApp.Domain.Note;
using Microsoft.AspNetCore.Mvc;

namespace IlnarApp.Api.Controllers;


public class NoteTypesController(INoteTypeRepository noteTypeRepository) : BaseController
{
	[HttpGet]
	[Route("{id:guid}")]
	public async Task<IActionResult> GetAsync(Guid id)
	{
		var noteType = await GetById(id);

		if (noteType == null)
		{
			throw new EntityNotFoundException("Тип записи не найден");
		}
		
		return Ok(noteType);
	}
	
	
	[HttpGet]
	[Route("")]
	public async Task<IActionResult> GetListAsync([FromQuery] int offset, [FromQuery] int limit)
	{
		var notesTypesLimit = limit is 0 or > 10 ? 10 : limit;
		
		var noteTypes = await noteTypeRepository.GetListAsync(offset, notesTypesLimit, null);
		
		var noteTypesCount = await noteTypeRepository.GetEntitiesCountAsync(null);
		
		var response = new PaginationData
		{
			Data = noteTypes,
			Pagination = new Paginator(noteTypesCount, offset, notesTypesLimit)
		};
		
		return Ok(response);
	}

	
	[HttpPost]
	[Route("add")]
	[ValidationAction]
	public async Task<IActionResult> InsertAsync([FromBody] NoteTypeData noteTypeData)
	{
		var noteType = new NoteType{Title = noteTypeData.Title};
		
		return Ok(await noteTypeRepository.InsertAsync(noteType));
	}
	
	
	[HttpPut]
	[Route("edit/{id:guid}")]
	[ValidationAction]
	public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] NoteTypeData noteTypeData)
	{
		var noteType = await GetById(id);

		if (noteType == null)
		{
			throw new EntityNotFoundException("Тип записи не найден");
		}
		
		noteType.Title = noteTypeData.Title;
		
		return Ok(await noteTypeRepository.UpdateAsync(noteType));
	}

	
	[HttpDelete]
	[Route("delete/{id:guid}")]
	public async Task<IActionResult> DeleteAsync(Guid id)
	{
		var noteType = await GetById(id);

		if (noteType == null)
		{
			throw new EntityNotFoundException("Тип записи не найден");
		}

		var result = await noteTypeRepository.DeleteAsync(noteType);

		var message = result ? "Тип записи успешно удален" : "Ошибка при удалении типа записи";

		var errorsList = new List<string> { message };

		var response = new ResponseData
		{
			Messages = errorsList,
			Success = result
		};

		return result ? Ok(response) : BadRequest(response);
	}

	private async Task<NoteType?> GetById(Guid id)
	{
		return await noteTypeRepository.GetAsync(id, null);
	}
}