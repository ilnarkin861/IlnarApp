using IlnarApp.Api.Actions;
using IlnarApp.Application.Exceptions;
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
		var archivesLimit = limit is 0 or > 10 ? 10 : limit;
		
		var archives = await archiveRepository.GetListAsync(offset, archivesLimit, null);

		var archivesCount = await archiveRepository.GetEntitiesCountAsync(null);

		var response = new PaginationData
		{
			Data = archives,
			Pagination = new Paginator(archivesCount, offset, archivesLimit)
		};
		
		return Ok(response);
	}
	
	
	[HttpPost]
	[Route("add")]
	[ValidationAction]
	public async Task<IActionResult> InsertAsync([FromBody] ArchiveRequest archiveRequest)
	{
		var archive = new Archive{Title = archiveRequest.Title};
		
		return Ok(await archiveRepository.InsertAsync(archive));
	}
	
	
	[HttpPut]
	[Route("edit/{id:guid}")]
	[ValidationAction]
	public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] ArchiveRequest archiveRequest)
	{
		var archive = await GetById(id);

		if (archive == null)
		{
			throw new EntityNotFoundException("Архив не найден");
		}
		
		archive.Title = archiveRequest.Title;
		
		return Ok(await archiveRepository.UpdateAsync(archive));
	}
	
	
	[HttpDelete]
	[Route("delete/{id:guid}")]
	public async Task<IActionResult> DeleteAsync(Guid id)
	{
		var archive = await GetById(id);

		if (archive == null)
		{
			throw new EntityNotFoundException("Архив не найден");
		}
		
		archive.Deleted = true;

		var result = await archiveRepository.DeleteAsync(archive);

		var message = result ? "Архив успешно удален" : "Ошибка при удалении архива";

		var errorsList = new List<string> { message };

		var response = new ResponseData
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