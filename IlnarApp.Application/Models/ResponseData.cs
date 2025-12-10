namespace IlnarApp.Application.Models;


public class ResponseData
{
	public bool Success { get; set; }
	public List<string> Messages { get; set; } = [];
}