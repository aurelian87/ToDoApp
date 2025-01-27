using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ToDoApp.Client;
using ToDoApp.Client.Extensions;
using ToDoApp.Client.Services;
using ToDoApp.Shared.AutoMapper;
using Refit;
using ToDoApp.Client.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using ToDoApp.Client.Shared;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddLocalization();
builder.Services.AddAutoMapper(typeof(ModelsProfile));
builder.Services.AddTransient<IAuthTokenStore, AuthTokenStore>();
builder.Services.AddTransient<AuthHeaderHandler>();
//builder.Services.AddScoped<IToDoService, ToDoService>();

builder.Services.AddRefitClient<IAuthService>().ConfigureHttpClient(httpClient => httpClient.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
builder.Services.AddRefitClient<IUserCredentialService>().ConfigureHttpClient(httpClient => httpClient.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
//builder.Services.AddRefitClient<IToDoService>().ConfigureHttpClient(httpClient => httpClient.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
//builder.Services.AddRefitClient<IUserProfileService>().ConfigureHttpClient(httpClient => httpClient.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

//builder.Services.AddRefitClient<IToDoService>().ConfigureHttpClient(httpClient => httpClient.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
//	.AddHttpMessageHandler<AuthHeaderHandler>();
builder.Services.AddRefitClient<IUserProfileService>().ConfigureHttpClient(httpClient => httpClient.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
	.AddHttpMessageHandler<AuthHeaderHandler>();
builder.Services.AddRefitClient<ITodoService>().ConfigureHttpClient(httpClient => httpClient.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
	.AddHttpMessageHandler<AuthHeaderHandler>();

builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<CustomAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(s => s.GetRequiredService<CustomAuthenticationStateProvider>());

//await builder.Build().RunAsync();

var host = builder.Build();
await host.SetDefaultCulture();
await host.RunAsync();