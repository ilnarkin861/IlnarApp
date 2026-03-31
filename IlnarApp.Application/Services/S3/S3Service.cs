using Amazon.S3;
using Amazon.S3.Model;
using IlnarApp.Application.Exceptions;
using IlnarApp.Application.Options;
using IlnarApp.Domain.Note;
using IlnarApp.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace IlnarApp.Application.Services.S3;


public class S3Service(
    ApplicationDbContext context, 
    IAmazonS3 amazonS3, 
    IOptions<S3Options> s3Options) : IS3Service<NoteImage>
{
    private readonly S3Options _options = s3Options.Value;
    private DbSet<NoteImage> GetDbSet() => context.Set<NoteImage>();
    
    
    public async Task<string> UploadFileAsync(IFormFile file)
    {
        var uniqueKey = $"{Guid.NewGuid()}_{file.FileName}";
        
        var isImage = file.ContentType.Contains("image", StringComparison.OrdinalIgnoreCase);
        
        try 
        {
            await using var compressedStream = isImage ? await CompressImageAsync(file) : file.OpenReadStream();
            
            var putRequest = new PutObjectRequest
            {
                BucketName = _options.BucketName,
                Key = uniqueKey,
                InputStream = compressedStream,
                ContentType = isImage ? "image/jpeg" : file.ContentType
            };
            
            await amazonS3.PutObjectAsync(putRequest);
        }
        catch (Exception) 
        {
            throw new ApiException("Не удалось загрузить файл в хранилище");
        }
        
        try
        {
            var fileUrl = $"{_options.Url?.TrimEnd('/')}/{_options.BucketName}/{uniqueKey}";
        
            var noteImage = new NoteImage 
            { 
                Url = fileUrl,
                Deleted = false
            };
    
            GetDbSet().Add(noteImage);
            
            await context.SaveChangesAsync();

            return noteImage.Url;
        }
        catch (DbUpdateException)
        {
            await amazonS3.DeleteObjectAsync(_options.BucketName, uniqueKey);

            throw new ApiException("Ошибка при сохранении информации о файле в базу данных");
        }
    }

    
    public async Task<List<string>> UploadFileAsync(IFormFile[] files)
    {
        var uploadedEntities = new List<NoteImage>();
        
        var uploadedKeys = new List<string>();

        try
        {
            foreach (var file in files)
            {
                var isImage = file.ContentType.Contains("image", StringComparison.OrdinalIgnoreCase);
                
                var uniqueKey = $"{Guid.NewGuid()}_{file.FileName}";
                
                await using var uploadStream = isImage ? await CompressImageAsync(file) : file.OpenReadStream();
            
                var putRequest = new PutObjectRequest
                {
                    BucketName = _options.BucketName,
                    Key = uniqueKey,
                    InputStream = uploadStream,
                    ContentType = isImage ? "image/jpeg" : file.ContentType
                };
            
                await amazonS3.PutObjectAsync(putRequest);
                
                uploadedKeys.Add(uniqueKey);
            
                var fileUrl = $"{_options.Url?.TrimEnd('/')}/{_options.BucketName}/{uniqueKey}";
                
                uploadedEntities.Add(new NoteImage { Url = fileUrl, Deleted = false });
            }
            
            GetDbSet().AddRange(uploadedEntities);
            
            await context.SaveChangesAsync();

            return uploadedEntities.Select(x => x.Url).ToList();
        }
        catch (Exception)
        {
            foreach (var s3Key in uploadedKeys)
            {
                try { await amazonS3.DeleteObjectAsync(_options.BucketName, s3Key); } 
                catch { /* Игнорируем ошибки при удалении во время отката */ }
            }

            throw new ApiException("Не удалось загрузить пакет файлов. Операция отменена.");
        }
    }
    

    public async Task<bool> DeleteFileAsync(Guid id)
    {
        var entity = await GetDbSet().FindAsync(id);
        
        if (entity == null) throw new ApiException("Не найден файл для удаления");

        entity.Deleted = true;
        
        return await context.SaveChangesAsync() > 0;
    }
    

    public async Task<bool> DeleteFileAsync(Guid[] ids)
    {
        var entities = await GetDbSet()
            .Where(x => ids.Contains(x.Id))
            .ToListAsync();

        foreach (var entity in entities)
        {
            entity.Deleted = true;
        }

        return await context.SaveChangesAsync() > 0;
    }
    

    public async Task<List<NoteImage>> GetFilesListAsync(int offset, int limit)
    {
        return await GetDbSet()
            .Where(x => !x.Deleted)
            .OrderByDescending(x => x.CreatedAt)
            .Skip(offset)
            .Take(limit)
            .ToListAsync();
    }

    
    public async Task<int> GetFilesCountAsync()
    {
        return await GetDbSet().Where(e => e.Deleted == false).CountAsync();
    }


    private async Task<Stream> CompressImageAsync(IFormFile file)
    {
        var outputStream = new MemoryStream(); 

        try 
        {
            await using (var inputStream = file.OpenReadStream())
         
            using (var image = await Image.LoadAsync(inputStream))
            {
                const int maxWidth = 1920;
            
                if (image.Width > maxWidth)
                {
                    image.Mutate(x => x.Resize(new ResizeOptions
                    {
                        Size = new Size(maxWidth, 0),
                        Mode = ResizeMode.Max
                    }));
                }
                
                await image.SaveAsJpegAsync(outputStream, new JpegEncoder
                {
                    Quality = 75 
                });
            }

            outputStream.Position = 0;
            
            return outputStream;
        }
        catch
        {
            await outputStream.DisposeAsync();
            
            throw;
        }
    }
}