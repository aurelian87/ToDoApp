using Microsoft.AspNetCore.Components;
using ToDoApp.Shared.Models;

namespace ToDoApp.Client.Pages;

public partial class AddToDo
{
    #region Fields

    private TodoModel _model;

    private TodoModel _modelClone;

    #endregion //Fields

    #region Parameters

    [Parameter] public int ToDoId { get; set; }

    #endregion //Parameters

    #region Private Properties

    [Inject] protected NavigationManager? NavigationManager { get; set; }

    private TodoModel Model
    {
        get
        {
            return _model;
        }
        set
        {
            _model = value;
            _modelClone = new TodoModel
            {
                Id = value.Id,
                Title = value.Title,
                Description = value.Description,
                DueDate = value.DueDate
            };
        }
    }

    private bool ShowCancelPopup { get; set; }

    #endregion //Private Properties 

    #region Private Methods

    protected override async Task OnInitializedAsync()
    {
        if (ToDoId > 0)
        {
            Model = ToDos.ToDosList.Find(x => x.Id == ToDoId);
        }
        else
        {
            Model = new TodoModel
            {
               DueDate = DateTime.Now
            };
        }

        await base.OnInitializedAsync();
    }

    private void Save()
    {
        if (Model.Id == 0)
        {
            if (ToDos.ToDosList.Count == 0)
            {
                Model.Id = 1;
            }
            else
            {
                var max = 0;
                foreach (var item in ToDos.ToDosList)
                {
                    if (item.Id > max)
                    {
                        max = item.Id;
                    }
                }
                Model.Id = max + 1;
            }
            ToDos.ToDosList.Add(Model);
        }
        NavigationManager?.NavigateTo("/todos");
    }

    private void Cancel()
    {
        if (Model.Title != _modelClone.Title || Model.Description != _modelClone.Description || Model.DueDate != _modelClone.DueDate)
        {
            ShowCancelPopup = true;
        }
        else
        {
            NavigationManager?.NavigateTo("/todos");
        }
    }

    private void Reset()
    {
        Model.Title = _modelClone.Title;
        Model.Description = _modelClone.Description;
        Model.DueDate = _modelClone.DueDate;

        NavigationManager?.NavigateTo("/todos");
    }

    #endregion //Private Methods
}