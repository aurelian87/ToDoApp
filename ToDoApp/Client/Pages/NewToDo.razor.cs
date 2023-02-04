using AutoMapper;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System.Net.Http.Json;
using ToDoApp.Shared.Models;
using ToDoApp.Shared.Validations;

namespace ToDoApp.Client.Pages;

public partial class NewToDo
{
    #region Fields

    private FluentValidationValidator? _fluentValidationValidator;

    #endregion //Fields

    #region Parameters

    [Parameter] public int ToDoId { get; set; }

    #endregion //Parameters

    #region Private Properties

    [Inject] private HttpClient Http { get; set; }

    [Inject] protected NavigationManager? NavigationManager { get; set; }

    [Inject] private IMapper Mapper { get; set; }

    private ToDoModel Model { get; set; } = new();

    private ToDoModel ModelClone { get; set; } = new();

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
            ModelClone = Mapper.Map(Model, ModelClone);
        }
        else
        {
            Model = new ToDoModel
            {
                DueDate = DateTime.Now
            };
            ModelClone = Mapper.Map(Model, ModelClone);
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
        var defaultData = JsonConvert.SerializeObject(Model);
        var newData = JsonConvert.SerializeObject(ModelClone);

        if (defaultData != newData)
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
        Model = ModelClone;
        NavigationManager?.NavigateTo("/");
    }

    #endregion //Private Methods
}