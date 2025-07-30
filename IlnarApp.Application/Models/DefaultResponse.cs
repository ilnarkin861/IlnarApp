namespace IlnarApp.Application.Models;


public class DefaultResponse
{
	public bool Success { get; set; }
	public List<string> Messages { get; set; } = [];
}