using IlnarApp.Application.Exceptions;
using IlnarApp.Application.Helpers;
using IlnarApp.Application.Models;
using IlnarApp.Application.Services.S3;
using IlnarApp.Domain.Note;
using Microsoft.AspNetCore.Mvc;

namespace IlnarApp.Api.Controllers;


public class FilesController(IS3Service<NoteImage> s3Service) : BaseController
{
    [HttpPost]
    [Route("upload")]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {

        if (file == null || file.Length == 0)
        {
            throw new ApiException("Нет файла");
        }
        
        var result = await s3Service.UploadFileAsync(file);
        
        return Ok(result);
    }
    
    
    [HttpPost]
    [Route("upload/multiple")]
    public async Task<IActionResult> UploadFile(IFormFile [] files)
    {

        if (files.Length == 0)
        {
            throw new ApiException("Должен быть выбран хотя бы один файл");
        }
        
        var result = await s3Service.UploadFileAsync(files);
        
        return Ok(result);
    }


    [HttpDelete]
    [Route("delete/{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var result = await s3Service.DeleteFileAsync(id);
        
        var response = new ResponseData
        {
            Messages = ["Файл успешно удален"],
            Success = result
        };
        
        return Ok(response);
    }
    
    
    [HttpDelete]
    [Route("delete")]
    public async Task<IActionResult> DeleteAsync([FromQuery] Guid[] ids)
    {
        if (ids == null || ids.Length == 0) throw new ApiException("Пустой query string");
        
        var result = await s3Service.DeleteFileAsync(ids);
    
        return Ok(new ResponseData { 
            Messages = [$"Удалено файлов: {ids.Length}"], 
            Success = result 
        });
    }


    [HttpGet]
    public async Task<IActionResult> GetListAsync([FromQuery] int offset, [FromQuery] int limit)
    {
        var filesLimit = limit is 0 or > 10 ? 10 : limit;
        
        var filesCount = await s3Service.GetFilesCountAsync();
        
        var files = await s3Service.GetFilesListAsync(offset, filesLimit);
        
        var response = new PaginationData
        {
            Data = files,
            Pagination = new Paginator(filesCount, offset, filesLimit)
        };
        
        return Ok(response);
    }
}