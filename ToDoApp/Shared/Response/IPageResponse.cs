namespace ToDoApp.Shared.Response
{
	public interface IPageResponse
	{
		int ItemsPerPage { get; }

		int ItemsCount { get; }

		int TotalItems { get; set; }

		int PageNumber { get; set; }

		int PageSize { get; set; }

		int TotalPages { get; }
	}
}