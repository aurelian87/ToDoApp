using ToDoApp.Shared.Models;
using ToDoApp.Client.Components;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using System.Reflection;
using Microsoft.AspNetCore.Components.Forms;
using System;

namespace ToDoApp.Client.Pages;

public partial class NewToDo
{
    #region Private Properties

    [Inject] private HttpClient Http { get; set; }

    [Inject] protected NavigationManager? NavigationManager { get; set; }

    private ToDoModel Model { get; set; } = new ToDoModel();

    private List<ToDoModel> ToDos { get; set; } = new List<ToDoModel>();

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
        ToDos = await Http.GetFromJsonAsync<List<ToDoModel>>("/api/ToDos");

        if (ToDoId > 0)
        {
            Model = ToDos.Find(x => x.Id == ToDoId);
            EditContext = new EditContext(Model);
            EditContext.OnFieldChanged += EditContext_OnFieldChanged;
        }
        else
        {
            Model = new ToDoModel();
            Model.DueDate = DateTime.Now;
            EditContext = new EditContext(Model);
            EditContext.OnFieldChanged += EditContext_OnFieldChanged;
        }

        await base.OnInitializedAsync();
    }

    // Note: The OnFieldChanged event is raised for each field in the model
    private void EditContext_OnFieldChanged(object sender, FieldChangedEventArgs e)
    {
        Console.WriteLine(e.FieldIdentifier.FieldName);
        if (ToDoId > 0 && !string.IsNullOrEmpty(e.FieldIdentifier.FieldName))
        {
            IsModified = true;
        }
        else
        {
            IsModified = false;
        }
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
        Console.WriteLine(IsModified);
        if (!IsModified)
        {
            NavigationManager?.NavigateTo("/");
            ShowCancelPopup = false;
        }
        else
        {
            ShowCancelPopup = true;
        }
    }

    #endregion //Private Methods
}