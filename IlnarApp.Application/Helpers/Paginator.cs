namespace IlnarApp.Application.Helpers;


public class Paginator(int count, int offset, int limit)
{
	private int Offset { get; } = offset;
	private int Limit { get; } = limit;
	public int Count { get; } = count;
	public bool HasPreviousPage => Offset > 0 || Offset > Limit;
	public bool HasNextPage => Count > Offset + Limit;
}