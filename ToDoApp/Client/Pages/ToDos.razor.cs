using Microsoft.AspNetCore.Components;
using ToDoApp.Shared.Models;

namespace ToDoApp.Client.Pages;

public partial class ToDos
{
    #region Public Properties

    public static List<TodoModel> ToDosList { get; set; } = new List<TodoModel>();

    #endregion //Public Properties

    #region Private Properties

    [Inject] protected NavigationManager? NavigationManager { get; set; }

    private int SelectedToDoId { get; set; }

    #endregion //Private Properties

    #region Private Methods

    protected override async Task OnInitializedAsync()
    {
        ToDosList = ToDosList.OrderByDescending(x => x.Id).ToList();

        if (ToDosList.Count > 0)
        {
            SelectedToDoId = ToDosList[0].Id;
        }

        await base.OnInitializedAsync();
    }

    private void Delete(int id)
    {
        for (int i = ToDosList.Count - 1; i >= 0; i--)
        {
            if (ToDosList[i].Id == id)
            {
                ToDosList.RemoveAt(i);
            }
        }
    }

    private void Edit(int id)
    {
        NavigationManager?.NavigateTo($"todo/{id}");
    }

    private Task OnSelectedItem(TodoModel item)
    {
        SelectedToDoId = item.Id;
        return Task.CompletedTask;
    }

    #endregion //Private Methods
}