using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using ToDoApp.Shared.Models;

namespace ToDoApp.Client.Pages;

public partial class AddToDo
{
    #region Private Fields

    private ToDoModel _model;

    private ToDoModel _modelClone;

    #endregion //Private Fields

    #region Private Properties

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

    private EditContext? EditContext { get; set; }

    private bool IsModified { get; set; }

    private bool ShowCancelPopup { get; set; }

    #endregion //Private Properties 

    #region Parameters

    [Parameter] public int ToDoId { get; set; }

    #endregion //Parameters

    #region Private Methods

    protected override async Task OnInitializedAsync()
    {
        if (ToDoId > 0)
        {
            Model = ToDos.ToDosList.Find(x => x.Id == ToDoId);
            EditContext = new EditContext(Model);
            EditContext.OnFieldChanged += EditContext_OnFieldChanged;
        }
        else
        {
            Model = new ToDoModel();
            Model.DueDate = DateTime.Now;
            EditContext = new EditContext(Model);
            EditContext.OnFieldChanged += EditContext_OnFieldChanged;

            Console.WriteLine($"Model Id: {Model.Id}");
            Console.WriteLine($"ToDoId: {ToDoId}");
        }

        await base.OnInitializedAsync();
    }

    // Note: The OnFieldChanged event is raised for each field in the model
    private void EditContext_OnFieldChanged(object sender, FieldChangedEventArgs e)
    {
        //Console.WriteLine(e.FieldIdentifier.FieldName);
        if (ToDoId > 0 && !string.IsNullOrEmpty(e.FieldIdentifier.FieldName))
        {
            IsModified = true;
        }
        else
        {
            IsModified = false;
        }
    }

    private void Save()
    {
        if (Model.Id == 0)
        {
            Model.Id = ToDos.ToDosList.Count + 1;
            ToDos.ToDosList.Add(Model);
        }
        NavigationManager?.NavigateTo("/todos");
    }

    private void Cancel()
    {
        if (!IsModified)
        {
            NavigationManager?.NavigateTo("/todos");
            ShowCancelPopup = false;
        }
        else
        {
            ShowCancelPopup = true;
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