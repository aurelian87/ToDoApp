using ToDoApp.Shared.Search;

namespace ToDoApp.Shared.Requests;

public class PageRequest
{
    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;

    public string? OrderBy { get; set; }

    public List<SearchFilter> SearchFilters { get; set; } = new();

    public FilterJunction FilterJunction { get; set; }
}