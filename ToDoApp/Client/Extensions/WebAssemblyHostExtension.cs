using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Globalization;

namespace ToDoApp.Client.Extensions;

public static class WebAssemblyHostExtension
{
    public async static Task SetDefaultCulture(this WebAssemblyHost host)
    {
        var storageService = host.Services.GetRequiredService<ILocalStorageService>();
        var result = await storageService.GetItemAsync<string>("language");

        CultureInfo culture;

        if (result != null)
        {
            culture = new CultureInfo(result);
        }
        else
        {
            await storageService.SetItemAsync("language", "ro-RO");
            culture = new CultureInfo("ro-RO");
        }

        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;
    }
}