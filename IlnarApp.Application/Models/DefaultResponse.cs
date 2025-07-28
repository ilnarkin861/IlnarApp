namespace IlnarApp.Application.Models;


public class DefaultResponse
{
	public bool Success { get; set; }
	public int StatusCode { get; set; }
	public List<string> ErrorMessages { get; set; } = [];
}