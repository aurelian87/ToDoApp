using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ToDoApp.Client;
using ToDoApp.Client.Extensions;
using ToDoApp.Client.Services;
using ToDoApp.Shared.AutoMapper;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddLocalization();
builder.Services.AddAutoMapper(typeof(ModelsProfile));
builder.Services.AddScoped<IToDoService, ToDoService>();

//await builder.Build().RunAsync();

var host = builder.Build();
await host.SetDefaultCulture();
await host.RunAsync();