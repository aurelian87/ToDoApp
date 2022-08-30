using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using System.Globalization;
using ToDoApp.Shared.ResourceFiles;

namespace ToDoApp.Client.Shared;

public partial class MainLayout
{
    #region Private Properties

    [Inject] private ILocalStorageService localStorage { get; set; }

    [Inject] private NavigationManager NavigationManager { get; set; }

    [Inject] private IStringLocalizer<Resource> Localizer { get; set; }

    private string LanguageSelected { get; set; }

    #endregion //Private Properties

    #region Private Methods

    protected override async Task OnInitializedAsync()
    {
        LanguageSelected = await localStorage.GetItemAsync<string>("language");

        await base.OnInitializedAsync();

    }

    private async Task Change(ChangeEventArgs e)
    {
        var language = e.Value.ToString();

        await localStorage.SetItemAsync("language", language);

        LanguageSelected = await localStorage.GetItemAsync<string>("language");

        CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(LanguageSelected);
        CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(LanguageSelected);

        NavigationManager.NavigateTo(NavigationManager.Uri, forceLoad: true);
    }

    #endregion //Private Methods
}