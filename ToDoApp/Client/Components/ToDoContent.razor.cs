using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using ToDoApp.Shared.Models;

namespace ToDoApp.Client.Components;

public partial class ToDoContent
{
    #region Fields

    public static List<ToDoModel> _toDos = new List<ToDoModel>();

    #endregion //Fields

    #region Private Properties

    [Inject] private HttpClient Http { get; set; }

    private List<ToDoModel> ToDos
    {
        get => _toDos;
        set => _toDos = value;
    }

    #endregion //Private Properties


    //protected override async Task OnInitializedAsync()
    //{
    //    ToDos = new List<ToDoModel>();

    //    ToDos = await Http.GetFromJsonAsync<List<ToDoModel>>("https://localhost:7212/api/ToDos");
    //    ToDos = ToDos.OrderByDescending(x => x.Id).ToList();

    //    await base.OnInitializedAsync();
    //}
}