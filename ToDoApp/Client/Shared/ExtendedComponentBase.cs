using AutoMapper;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;

namespace ToDoApp.Client.Shared;

public abstract class ExtendedComponentBase : ComponentBase
{
    #region Private Properties

    [CascadingParameter] protected MainLayout? MainLayout { get; set; }

	[Inject] protected NavigationManager? NavigationManager { get; set; }

	[Inject] protected ILocalStorageService LocalStorageService { get; set; }

    [Inject] protected IMapper? Mapper { get; set; }

    #endregion //Private Properties

    #region Private Methods

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
    }

    protected void ShowConsoleMessage(string message)
    {
        Console.WriteLine(message);
    }

    #endregion //Private Methods
}