using EntityFrameworkCore.UseRowNumberForPaging;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;
using ToDoApp.ApplicationLayer.Services;
using ToDoApp.ApplicationLayer.Services.Contracts;
using ToDoApp.PersistenceLayer.Data;
using ToDoApp.PersistenceLayer.Repositories;
using ToDoApp.Server.Extensions;
using ToDoApp.Shared.Models;
using ToDoApp.Shared.ModelValidators;
using ToDoApp.Shared.Repositories;
using ToDoApp.ApplicationLayer.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();


var connectionString = builder.Configuration.GetConnectionString("ToDosAppConnectionString");
builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(connectionString, options => options.UseRowNumberForPaging()));
builder.RegisterPresentation();


//builder.Services.AddScoped<IValidator<UserProfileModel>, UserProfileModelValidator>();
//builder.Services.AddScoped<IToDoService, ToDoService>();
//builder.Services.AddScoped<IToDoRepository, ToDoRepository>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is my super secret")),
			ValidateIssuer = false,
			ValidateAudience = false
		};
	});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseWebAssemblyDebugging();
}
else
{
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();


app.UseRouting();


app.MapRazorPages();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();