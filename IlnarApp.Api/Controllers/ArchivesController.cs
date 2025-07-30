using IlnarApp.Api.Actions;
using IlnarApp.Api.Exceptions;
using IlnarApp.Application.Helpers;
using IlnarApp.Application.Models;
using IlnarApp.Domain.Archive;
using Microsoft.AspNetCore.Mvc;

namespace IlnarApp.Api.Controllers;


public class ArchivesController(IArchiveRepository archiveRepository) : BaseController
{
	[HttpGet]
	[Route("{id:guid}")]
	public async Task<IActionResult> GetAsync(Guid id)
	{
		var archive = await GetById(id);

		if (archive == null)
		{
			throw new EntityNotFoundException("Архив не найден");
		}
		
		return Ok(archive);
	}
	
	
	[HttpGet]
	[Route("")]
	public async Task<IActionResult> GetListAsync([FromQuery] int offset, [FromQuery] int limit)
	{
		var archivesLimit = limit is 0 or > 15 ? 15 : limit;
		
		var tags = await archiveRepository.GetListAsync(offset, archivesLimit, null);

		var archivesCount = await archiveRepository.GetEntitiesCountAsync(null);

		var response = new PaginationResponse
		{
			Data = tags,
			Pagination = new Paginator(archivesCount, offset, archivesLimit)
		};
		
		return Ok(response);
	}
	
	
	[HttpPost]
	[Route("add")]
	[ValidationAction]
	public async Task<IActionResult> InsertAsync([FromBody] ArchiveDto archiveDto)
	{
		var archive = new Archive{Title = archiveDto.Title};
		
		return Ok(await archiveRepository.InsertAsync(archive));
	}
	
	
	[HttpPut]
	[Route("edit/{id:guid}")]
	[ValidationAction]
	public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] ArchiveDto archiveDto)
	{
		var archive = await GetById(id);

		if (archive == null)
		{
			throw new EntityNotFoundException("Архив не найден");
		}
		
		archive.Title = archiveDto.Title;
		
		return Ok(await archiveRepository.UpdateAsync(archive));
	}
	
	
	[HttpDelete]
	[Route("delete/{id:guid}")]
	public async Task<IActionResult> DeleteAsync(Guid id)
	{
		var tag = await GetById(id);

		if (tag == null)
		{
			throw new EntityNotFoundException("Архив не найден");
		}

		var result = await archiveRepository.DeleteAsync(tag);

		var message = result ? "Архив успешно удален" : "Ошибка при удалении архива";

		var errorsList = new List<string> { message };

		var response = new DefaultResponse
		{
			Messages = errorsList,
			Success = result
		};

		return result ? Ok(response) : BadRequest(response);
	}
	
	
	private async Task<Archive?> GetById(Guid id)
	{
		return await archiveRepository.GetAsync(id, null);
	}
}