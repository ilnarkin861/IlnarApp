using IlnarApp.Domain;
using IlnarApp.Domain.Tag;

namespace IlnarApp.Application.Models;


public class NoteFilterRequest : IEntityFilter
{
	public Guid? NoteTypeId { get; set; }
	public int? Year { get; set; }
	public int? Month { get; set; }
	public int? Day { get; set; }
	public Guid? ArchiveId { get; set; }
	public List<Guid>? TagIds { get; set; }
}