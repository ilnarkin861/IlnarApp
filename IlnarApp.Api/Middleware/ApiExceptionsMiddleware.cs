using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using IlnarApp.Api.Exceptions;
using IlnarApp.Application.Models;
using Microsoft.EntityFrameworkCore;

namespace IlnarApp.Api.Middleware;

public class ApiExceptionsMiddleware(RequestDelegate next, ILogger<ApiExceptionsMiddleware> logger)
{
	public async Task InvokeAsync(HttpContext context)
	{
		try
		{
			await next(context);
		}
		catch (Exception exception)
		{
			await HandleExceptionAsync(context, exception);
		}
	}
	
	private async Task HandleExceptionAsync(HttpContext context, Exception exception)
	{
		context.Response.ContentType = "application/json";
		context.Response.StatusCode = (int) HttpStatusCode.BadRequest;

		var response = new DefaultResponse
		{
			Success = false
		};

		switch (exception)
		{
			case ApiException restException:
				response.Messages.Add(restException.Message);
				break;

			case EntityNotFoundException entityNotFoundException:
				response.Messages.Add(entityNotFoundException.Message);
				break;
                
			case IOException:
				response.Messages.Add("File system operation error");
				break;
                
			case DbUpdateException:
				response.Messages.Add("Ошибка базы данных");
				logger.LogError(exception, exception.Message);
				break;
                
			default:
				context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
				response.Messages.Add("Internal Server Error");
				logger.LogError(exception, exception.Message);
				break;
		}
            
		var errorResponse = JsonSerializer.Serialize(response);
            
		await context.Response.WriteAsync(errorResponse, Encoding.UTF8);
	}
}