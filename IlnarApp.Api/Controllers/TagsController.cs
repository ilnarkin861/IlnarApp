using IlnarApp.Api.Actions;
using IlnarApp.Application.Exceptions;
using IlnarApp.Application.Helpers;
using IlnarApp.Application.Models;
using IlnarApp.Domain.Tag;
using Microsoft.AspNetCore.Mvc;

namespace IlnarApp.Api.Controllers;


public class TagsController(ITagRepository tagRepository) : BaseController
{
	[HttpGet]
	[Route("{id:guid}")]
	public async Task<IActionResult> GetAsync(Guid id)
	{
		var tag = await GetById(id);

		if (tag == null)
		{
			throw new EntityNotFoundException("Тег не найден");
		}
		
		return Ok(tag);
	}
	
	
	[HttpGet]
	[Route("")]
	public async Task<IActionResult> GetListAsync([FromQuery] int offset, [FromQuery] int limit)
	{
		var tagsLimit = limit is 0 or > 15 ? 15 : limit;
		
		var tags = await tagRepository.GetListAsync(offset, tagsLimit, null);

		var tagsCount = await tagRepository.GetEntitiesCountAsync(null);

		var response = new PaginationData
		{
			Data = tags,
			Pagination = new Paginator(tagsCount, offset, tagsLimit)
		};
		
		return Ok(response);
	}
	
	
	[HttpPost]
	[Route("add")]
	[ValidationAction]
	public async Task<IActionResult> InsertAsync([FromBody] TagRequest tagRequest)
	{
		var noteType = new Tag{Title = tagRequest.Title};
		
		return Ok(await tagRepository.InsertAsync(noteType));
	}

	
	[HttpPut]
	[Route("edit/{id:guid}")]
	[ValidationAction]
	public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] TagRequest tagRequest)
	{
		var tag = await GetById(id);

		if (tag == null)
		{
			throw new EntityNotFoundException("Тег не найден");
		}
		
		tag.Title = tagRequest.Title;
		
		return Ok(await tagRepository.UpdateAsync(tag));
	}
	
	
	[HttpDelete]
	[Route("delete/{id:guid}")]
	public async Task<IActionResult> DeleteAsync(Guid id)
	{
		var tag = await GetById(id);

		if (tag == null)
		{
			throw new EntityNotFoundException("Тег не найден");
		}

		var result = await tagRepository.DeleteAsync(tag);

		var message = result ? "Тег успешно удален" : "Ошибка при удалении тега";

		var errorsList = new List<string> { message };

		var response = new ResponseData
		{
			Messages = errorsList,
			Success = result
		};

		return result ? Ok(response) : BadRequest(response);
	}
	
	
	private async Task<Tag?> GetById(Guid id)
	{
		return await tagRepository.GetAsync(id, null);
	}
}