namespace GoFit.Application.Models;

public class PaginatedResult<T>
{
    public PaginatedResult(int pageNumber, int pageSize, int total, IReadOnlyList<T> items)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        Total = total;
        Items = items;
    }

    public int PageNumber { get; }
    public int PageSize { get; }
    public int Total { get; }
    public IReadOnlyList<T> Items { get; }
}