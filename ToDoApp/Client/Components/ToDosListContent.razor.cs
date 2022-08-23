using Microsoft.AspNetCore.Components;
using ToDoApp.Shared.Models;
using System.Net.Http.Json;

namespace ToDoApp.Client.Components;

public partial class ToDosListContent
{
    #region Private Properties

    [Inject] private HttpClient Http { get; set; }

    [Inject] protected NavigationManager? NavigationManager { get; set; }

    private List<ToDoModel> ToDos { get; set; }

    private ToDoModel todo { get; set; }

    private int SelectedToDoId { get; set; }

    #endregion //Private Properties

    #region Private Methods

    protected override async Task OnInitializedAsync()
    {
        todo = new ToDoModel();
        ToDos = new List<ToDoModel>();

        ToDos = await Http.GetFromJsonAsync<List<ToDoModel>>("/api/ToDos");
        ToDos = ToDos.OrderByDescending(x => x.Id).ToList();

        if (ToDos.Count > 0)
        {
            SelectedToDoId = ToDos[0].Id;
        }

        await base.OnInitializedAsync();
    }

    private async Task LoadToDos()
    {
        ToDos = await Http.GetFromJsonAsync<List<ToDoModel>>("/api/ToDos");
        ToDos = ToDos.OrderByDescending(x => x.Id).ToList();

        StateHasChanged();
    }

    private async Task Save()
    {
        if (todo.Id == 0)
        {
            await Http.PostAsJsonAsync("/api/ToDos", todo);
        }
        else
        {
            await Http.PutAsJsonAsync("/api/ToDos/" + todo.Id, todo);
        }

        await LoadToDos();
    }


    private async Task Delete(int id)
    {
        await Http.DeleteAsync("/api/ToDos/" + id);

        await LoadToDos();
    }

    private void Edit(int id)
    {
        NavigationManager?.NavigateTo($"toDos/{id}");
    }


    private Task OnSelectedItem(ToDoModel item)
    {
        SelectedToDoId = item.Id;
        return Task.CompletedTask;
    }

    #endregion //Private Methods
}