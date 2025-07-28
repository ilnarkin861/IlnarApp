using System.Collections;
using IlnarApp.Application.Helpers;


namespace IlnarApp.Application.Models;


public class PaginationResponse
{
	public required ICollection Data { get; set; }
	public required Paginator Pagination { get; set; }
}