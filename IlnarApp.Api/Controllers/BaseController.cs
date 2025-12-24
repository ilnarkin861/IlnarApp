using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IlnarApp.Api.Controllers;



[Route("[controller]")]
[Authorize]
public class BaseController : ControllerBase
{
	
}