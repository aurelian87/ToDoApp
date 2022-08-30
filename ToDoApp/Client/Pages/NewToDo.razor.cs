using ToDoApp.Shared.Models;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace ToDoApp.Client.Pages;

public partial class NewToDo
{
    #region Fields

    private ToDoModel _model;

    private ToDoModel _modelClone;

    #endregion //Fields

    #region Parameters

    [Parameter] public int ToDoId { get; set; }

    #endregion //Parameters

    #region Private Properties

    [Inject] private HttpClient Http { get; set; }

    [Inject] protected NavigationManager? NavigationManager { get; set; }

    private ToDoModel Model
    {
        get
        {
            return _model;
        }
        set
        {
            _model = value;
            _modelClone = new ToDoModel
            {
                Id = value.Id,
                Title = value.Title,
                Description = value.Description,
                DueDate = value.DueDate
            };
        }
    }

    private List<ToDoModel> ToDos { get; set; } = new List<ToDoModel>();

    private bool ShowCancelPopup { get; set; }

    #endregion //Private Properties 

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
            Model = new ToDoModel
            {
                DueDate = DateTime.Now
            };
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
        if (Model.Title != _modelClone.Title || Model.Description != _modelClone.Description || Model.DueDate != _modelClone.DueDate)
        {
            ShowCancelPopup = true;
        }
        else
        {
            NavigationManager?.NavigateTo("/");
        }
    }
    private void Reset()
    {
        Model.Title = _modelClone.Title;
        Model.Description = _modelClone.Description;
        Model.DueDate = _modelClone.DueDate;

        NavigationManager?.NavigateTo("/");
    }

    #endregion //Private Methods
}