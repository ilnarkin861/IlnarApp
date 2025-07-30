using System.Net;
using IlnarApp.Application.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;


namespace IlnarApp.Api.Actions;


public class ValidationAction : ActionFilterAttribute
{
	public override void OnActionExecuting(ActionExecutingContext context)
	{
		var errorsList = new List<string>();
		
		if (context.ModelState.IsValid) return;
		
		foreach (var value in context.ModelState.Values)
		{
			errorsList.AddRange(value.Errors.Select(error => error.ErrorMessage));
		}

		var response = new DefaultResponse
		{
			Success = false,
			Messages = errorsList
		};
		
		context.Result = new BadRequestObjectResult(response);
	}
}