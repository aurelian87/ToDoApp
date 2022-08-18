using ToDoApp.Shared.Models;
using ToDoApp.Client.Components;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using System.Reflection;

namespace ToDoApp.Client.Pages;

public partial class NewToDo
{
    #region Private Properties

    [Inject] private HttpClient Http { get; set; }

    [Inject] protected NavigationManager? NavigationManager { get; set; }

    private ToDoModel Model { get; set; } = new ToDoModel();

    public List<ToDoModel> ToDos { get; set; } = new List<ToDoModel>();

    #endregion //Private Properties 

    #region Parameters

    [Parameter] public int ToDoId { get; set; }

    #endregion //Parameters

    #region Private Methods

    protected override async Task OnInitializedAsync()
    {
        ToDos = await Http.GetFromJsonAsync<List<ToDoModel>>("/api/ToDos");

        if (ToDoId > 0)
        {
            Model = ToDos.Find(x => x.Id == ToDoId);
        }
        else
        {
            Model = new ToDoModel();
            Model.DueDate = DateTime.Now;
        }

        await base.OnInitializedAsync();
    }

    private async Task Save()
    {
        if (Model.Id == 0)
        {
            await Http.PostAsJsonAsync("/api/ToDos", Model);
        }
        else
        {
            await Http.PutAsJsonAsync("/api/ToDos/" + Model.Id, Model);
        }

        NavigationManager?.NavigateTo("/");

    }

    private void Cancel()
    {
        NavigationManager?.NavigateTo("/");
    }
    #endregion //Private Methods
}