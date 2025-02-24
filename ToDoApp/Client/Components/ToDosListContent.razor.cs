using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using ToDoApp.Client.Services;
using ToDoApp.Shared.Models;
using ToDoApp.Shared.Requests;
using ToDoApp.Shared.Response;
using ToDoApp.Shared.Search;

namespace ToDoApp.Client.Components;

public partial class TodosListContent
{
	#region Private Properties

	[Inject] private ITodoService? ToDoService { get; set; }

	private PaginatedResponse<TodoModel>? PaginatedResponse { get; set; }

	private List<TodoModel> ToDos => PaginatedResponse?.Data ?? new();

	private int SelectedTodoId { get; set; }

	private string? SearchTerm { get; set; }

	#endregion //Private Properties

	#region Private Methods

	protected override async Task OnInitializedAsync()
	{
		await LoadToDos();

		if (ToDos.Count > 0)
		{
			SelectedTodoId = ToDos[0].Id;
		}

		await base.OnInitializedAsync();
	}

	private async Task LoadToDos()
	{
		MainLayout?.ShowPageLoader();
		PaginatedResponse = await ToDoService!.GetPaginatedResult(GetPageRequest());
		MainLayout?.HidePageLoader();
	}

	private void Add()
	{
		NavigationManager?.NavigateTo($"{PageRoute.Todos}/0");
	}

	private async Task Delete(int id)
	{
		MainLayout?.ShowPageLoader();
		await ToDoService!.Delete(id);
		await LoadToDos();
		MainLayout?.HidePageLoader();
	}

	private void Edit(int id)
	{
		NavigationManager?.NavigateTo($"{PageRoute.Todos}/{id}");
	}

	private Task OnSelectedItem(TodoModel item)
	{
		SelectedTodoId = item.Id;
		return Task.CompletedTask;
	}

	private async Task Search()
	{
		if (!string.IsNullOrEmpty(SearchTerm))
		{
			await LoadToDos();
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
		PaginatedResponse = await ToDoService!.GetPaginatedResult(GetPageRequest(pageNumber));
	}

	private async Task GetPageNumber(int pageNumber)
	{
		PaginatedResponse!.PageNumber = pageNumber;
		await GetPaginatedResults(pageNumber);
	}


	private static PageRequest GetPageRequest(int pageNumber = 1)
	{
		var pageRequest = new PageRequest
		{
			PageNumber = pageNumber,
			//PageSize = 2,
			OrderBy = $"{nameof(TodoModel.DueDate)}"
			//SearchFilters = new()
			//{
			//	new()
			//	{
			//		PropertyName = nameof(TodoModel.Title),
			//		PropertyValue = "fi",
			//		Operator = FilterOperator.StartsWith
			//	}
			//},
			//FilterJunction = FilterJunction.AND
		};

		return pageRequest;
	}

	#endregion //Private Methods
}