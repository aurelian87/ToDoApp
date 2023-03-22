using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using ToDoApp.Client.Services;
using ToDoApp.Shared.Models;
using ToDoApp.Shared.Requests;
using ToDoApp.Shared.Response;

namespace ToDoApp.Client.Components;

public partial class ToDosListContent
{
	#region Private Properties

	[Inject] private IToDoService? ToDoService { get; set; }

	private PageResponse<ToDoModel>? PageResponse { get; set; }

	private List<ToDoModel> ToDos => PageResponse?.Data ?? new();

	//private List<ToDoModel> ToDosPag => PageResponse?.Data ?? new();

	private int SelectedToDoId { get; set; }

	private string? SearchTerm { get; set; }

	#endregion //Private Properties

	#region Private Methods

	protected override async Task OnInitializedAsync()
	{
		await LoadToDos();

		if (ToDos.Count > 0)
		{
			SelectedToDoId = ToDos[0].Id;
		}

		//PageResponse = await ToDoService!.GetPaginatedResult(GetPageRequest());

		await base.OnInitializedAsync();
	}

	private async Task LoadToDos()
	{
		//ToDos = await ToDoService!.GetAll();
		//ToDos = ToDos.OrderByDescending(x => x.Id).ToList();
		PageResponse = await ToDoService!.GetPaginatedResult(GetPageRequest());
	}

	private void Add()
	{
		NavigationManager?.NavigateTo($"{PageRoute.Todos}/0");
	}

	private async Task Delete(int id)
	{
		await ToDoService!.Delete(id);
		await LoadToDos();
	}

	private void Edit(int id)
	{
		NavigationManager?.NavigateTo($"{PageRoute.Todos}/{id}");
	}

	private Task OnSelectedItem(ToDoModel item)
	{
		SelectedToDoId = item.Id;
		return Task.CompletedTask;
	}

	private async Task Search()
	{
		if (!string.IsNullOrEmpty(SearchTerm))
		{
			await LoadToDos();
			//ToDos = ToDos.Where(item => item.Title.Contains(SearchTerm)).ToList();
		}
		else
		{
			await LoadToDos();
		}
	}

	private async Task OnEnter(KeyboardEventArgs e)
	{
		if (e.Key == "Enter")
		{
			await Search();
		}
	}

	private async Task GetPaginatedResults(int pageNumber)
	{
		PageResponse = await ToDoService!.GetPaginatedResult(GetPageRequest(pageNumber));
	}

	private async Task GetPageNumber(int pageNumber)
	{
		PageResponse!.PageNumber = pageNumber;
		await GetPaginatedResults(pageNumber);
	}


	private static PageRequest GetPageRequest(int pageNumber = 1)
	{
		var pageRequest = new PageRequest
		{
			PageNumber = pageNumber,
			PageSize = 5,
			OrderBy = "Id desc"
        };

		return pageRequest;
	}

	#endregion //Private Methods
}