﻿using Microsoft.AspNetCore.Components;
using ToDoApp.Shared.Models;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Web;
using ToDoApp.Client.Services;

namespace ToDoApp.Client.Components;

public partial class ToDosListContent
{
    #region Private Properties

    [Inject] private HttpClient Http { get; set; }

    [Inject] private IToDoService ToDoService { get; set; }

    private List<ToDoModel> ToDos { get; set; }

    private ToDoModel todo { get; set; }

    private int SelectedToDoId { get; set; }

    private string SearchTerm { get; set; }

    #endregion //Private Properties

    #region Private Methods

    protected override async Task OnInitializedAsync()
    {
        todo = new ToDoModel();
        ToDos = new List<ToDoModel>();

        //ToDos = await Http.GetFromJsonAsync<List<ToDoModel>>("/api/ToDos");
        var todoById = ToDoService.GetById();

        ToDos = await ToDoService.GetAll(); 
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


    private async Task Search()
    {
        if (!string.IsNullOrEmpty(SearchTerm))
        {
            ToDos = await Http.GetFromJsonAsync<List<ToDoModel>>("/api/ToDos");
            ToDos = ToDos.Where(item => item.Title.Contains(SearchTerm)).ToList();
        }
        else
        {
            await LoadToDos();
        }
    }

    private async Task OnEnter(KeyboardEventArgs e)
    {
        if (e.Key == "Enter")
        {
            await Search();
        }
    }

    #endregion //Private Methods
}