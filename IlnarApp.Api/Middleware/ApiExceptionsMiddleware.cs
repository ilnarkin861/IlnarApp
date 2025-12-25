using System.Net;
using System.Text;
using System.Text.Json;
using IlnarApp.Application.Exceptions;
using IlnarApp.Application.Models;
using Microsoft.AspNetCore.Authentication;
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

		var response = new ResponseData
		{
			Success = false
		};
		
		var options = new JsonSerializerOptions {
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		};
		
		switch (exception)
		{
			case ApiException restException:
				response.Messages.Add(restException.Message);
				break;

			case EntityNotFoundException entityNotFoundException:
				context.Response.StatusCode = (int) HttpStatusCode.NotFound;
				response.Messages.Add(entityNotFoundException.Message);
				break;
			
			case AuthenticationFailureException authenticationFailureException:
				context.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
				response.Messages.Add(authenticationFailureException.Message);
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
            
		var errorResponse = JsonSerializer.Serialize(response, options);
            
		await context.Response.WriteAsync(errorResponse, Encoding.UTF8);
	}
}