using Microsoft.AspNetCore.Components;
using ToDoApp.Shared.Response;

namespace ToDoApp.Client.Shared;

public partial class Pagination
{
	#region Private Parameters

	[Parameter]
	public IPageResponse? PageResponse { get; set; }

	[Parameter]
	public EventCallback<int> OnPageChanged { get; set; }

	[Parameter]
	public string AriaLabel { get; set; }  = "pagination";

	#endregion //Private Parameters

	#region Private Properties

	private int PageNumber => PageResponse?.PageNumber ?? 0;


	private int TotalPages => PageResponse?.TotalPages ?? 0;


	private int ItemsPerPage => PageResponse?.ItemsPerPage ?? 0;


	private int ItemsCount => PageResponse?.ItemsCount ?? 0;


	private int TotalItems => PageResponse?.TotalItems ?? 0;

	#endregion //Private Properties

	#region Private Methods

	private async Task ChangePage(int pageNumber)
	{
		PageResponse!.PageNumber = pageNumber;
		await OnPageChanged.InvokeAsync(pageNumber);
	}

	#endregion //Private Methods
}