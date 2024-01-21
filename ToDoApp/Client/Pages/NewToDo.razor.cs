using AutoMapper;
using ChangeTracking;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using ToDoApp.Client.Services;
using ToDoApp.Shared.Models;
using ToDoApp.Shared.Validations;

namespace ToDoApp.Client.Pages;

[Route($"{PageRoute.TodoDetails}")]
public partial class NewToDo
{
    #region Fields

    private FluentValidationValidator? _fluentValidationValidator;
    private ToDoModel? _model;

    #endregion //Fields

    #region Parameters

    [Parameter] public int ToDoId { get; set; }

    #endregion //Parameters

    #region Private Properties

    [Inject] private IToDoService? ToDoService { get; set; }

    [Inject] private IMapper? Mapper { get; set; }

    //private ToDoModel Model { get; set; } = new();

    private ToDoModel Model
    {
        get
        {
            if (_model is null)
            {
                Model = new();
            }

            return _model!;
        }

        set
        {
            _model = value;

            if (_model is not IChangeTrackable)
            {
                _model = _model?.AsTrackable();
            }
        }
    }

    private ToDoModel ModelClone { get; set; } = new();

    private bool ShowCancelPopup { get; set; }

    #endregion //Private Properties 

    #region Private Methods

    protected override async Task OnInitializedAsync()
    {
        if (ToDoId > 0)
        {
            var todo = await ToDoService!.GetById(ToDoId);
            Model = todo;
            //ModelClone = Mapper!.Map(Model, ModelClone);
        }
        else
        {
            Model = new ToDoModel
            {
                DueDate = DateTime.Now
            };
            //ModelClone = Mapper!.Map(Model, ModelClone);
        }

        await base.OnInitializedAsync();
    }

    private async Task Save()
    {
        if (ToDoId == 0)
        {
            var isValid = await _fluentValidationValidator!.ValidateAsync();
            if (isValid)
            {
                await ToDoService!.Add(Model);
            }
        }
        else
        {
            var isValid = await _fluentValidationValidator!.ValidateAsync();
            if (isValid)
            {
                await ToDoService!.Update(ToDoId, Model);
            }
        }

        NavigationManager?.NavigateTo(PageRoute.Home);
    }

    private void Cancel()
    {
        var trackable = Model.CastToIChangeTrackable();

        if (trackable.IsChanged)
        {
            ShowCancelPopup = true;
        }
        else
        {
            NavigationManager?.NavigateTo(PageRoute.Home);
        }

        //var defaultData = JsonConvert.SerializeObject(Model);
        //var newData = JsonConvert.SerializeObject(ModelClone);

        //if (defaultData != newData)
        //{
        //    ShowCancelPopup = true;
        //}
        //else
        //{
        //    NavigationManager?.NavigateTo(PageRoute.Home);
        //}
    }

    private void Reset()
    {
        //Model = ModelClone;
        NavigationManager?.NavigateTo(PageRoute.Home);
    }

    #endregion //Private Methods
}