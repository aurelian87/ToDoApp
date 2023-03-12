using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using ToDoApp.Client.Services;
using ToDoApp.Shared.Models;

namespace ToDoApp.Client.Components;

public partial class ToDosListContent
{
	#region Private Properties

	[Inject] private IToDoService? ToDoService { get; set; }

	private List<ToDoModel> ToDos { get; set; } = new();

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

		await base.OnInitializedAsync();
	}

	private async Task LoadToDos()
	{
		ToDos = await ToDoService!.GetAll();
		ToDos = ToDos.OrderByDescending(x => x.Id).ToList();
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
			ToDos = ToDos.Where(item => item.Title.Contains(SearchTerm)).ToList();
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

	#endregion //Private Methods
}