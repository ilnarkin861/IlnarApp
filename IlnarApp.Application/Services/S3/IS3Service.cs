using IlnarApp.Domain;
using Microsoft.AspNetCore.Http;

namespace IlnarApp.Application.Services.S3;


public interface IS3Service<TEntity> where TEntity : Entity
{
    Task<string> UploadFileAsync(IFormFile file);
    Task<List<string>> UploadFileAsync(IFormFile [] files);
    Task<bool> DeleteFileAsync(Guid id);
    Task<bool> DeleteFileAsync(Guid [] ids);
    Task<List<TEntity>> GetFilesListAsync(int offset, int limit);
    Task<int> GetFilesCountAsync();
}