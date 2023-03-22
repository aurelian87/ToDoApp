namespace ToDoApp.Shared.Response
{
    public class PageResponse<T> : IPageResponse
    {
		public List<T>? Data { get; set; }

		public int ItemsPerPage => Data?.Count ?? 0;

		public int ItemsCount => (PageNumber - 1) * PageSize + ItemsPerPage;

		public int TotalItems { get; set; }

		public int PageNumber { get; set; }

		public int PageSize { get; set; }

		public int TotalPages => (int)Math.Ceiling(TotalItems / (double)PageSize);
	}
}