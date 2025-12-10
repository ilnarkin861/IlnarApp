using System.Collections;
using IlnarApp.Application.Helpers;


namespace IlnarApp.Application.Models;


public class PaginationData
{
	public required ICollection Data { get; set; }
	public required Paginator Pagination { get; set; }
}