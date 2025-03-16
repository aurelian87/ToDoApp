using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using ToDoApp.Client.Services;
using ToDoApp.Client.Shared;
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

	private TodoModel SelectedTodo { get; set; }

	private string? SearchTerm { get; set; }

    [CascadingParameter] protected MainLayout? MainLayout { get; set; }

    [Inject] protected NavigationManager? NavigationManager { get; set; }

    #endregion //Private Properties

    #region Private Methods

    protected override async Task OnInitializedAsync()
	{
		await LoadToDos();
	}

	private async Task LoadToDos()
	{
		MainLayout?.ShowPageLoader();
		PaginatedResponse = await ToDoService!.GetPaginatedResult(GetPageRequest());

		if (ToDos.Count > 0)
		{
			SelectedTodo = ToDos.FirstOrDefault() ?? new();
		}

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

	private void OnSelectedItem(TodoModel item)
	{
        SelectedTodo = item;
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
			OrderBy = $"{nameof(TodoModel.DueDate)}",
			FilterJunction = FilterJunction.AND,
            //SearchFilters = new()
            //{
            //	new()
            //	{
            //		PropertyName = nameof(TodoModel.Title),
            //		PropertyValue = "fi",
            //		Operator = FilterOperator.StartsWith
            //	}
            //}
        };

		return pageRequest;
	}

	#endregion //Private Methods
}