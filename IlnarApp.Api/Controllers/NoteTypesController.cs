using System.Net;
using IlnarApp.Api.Actions;
using IlnarApp.Api.Exceptions;
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
		return Ok(await noteTypeRepository.GetListAsync(0, 10, null));
	}

	
	[HttpPost]
	[Route("add")]
	[ValidationAction]
	public async Task<IActionResult> InsertAsync([FromBody] NoteTypeDto noteTypeDto)
	{
		var noteType = new NoteType{Title = noteTypeDto.Title};
		
		return Ok(await noteTypeRepository.InsertAsync(noteType));
	}
	
	
	[HttpPut]
	[Route("edit/{id:guid}")]
	[ValidationAction]
	public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] NoteTypeDto noteTypeDto)
	{
		var noteType = await GetById(id);

		if (noteType == null)
		{
			throw new EntityNotFoundException("Тип записи не найден");
		}
		
		noteType.Title = noteTypeDto.Title;
		
		return Ok(await noteTypeRepository.InsertAsync(noteType));
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

		var response = new DefaultResponse
		{
			ErrorMessages = errorsList,
			Success = result,
			StatusCode = result ? (int)HttpStatusCode.OK : (int)HttpStatusCode.BadRequest
		};

		return result ? Ok(response) : BadRequest(response);
	}

	private async Task<NoteType?> GetById(Guid id)
	{
		return await noteTypeRepository.GetAsync(id, null);
	}
}