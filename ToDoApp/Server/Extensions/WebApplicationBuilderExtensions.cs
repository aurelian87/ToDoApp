using ToDoApp.ApplicationLayer.Extensions;
using ToDoApp.PersistenceLayer.Extensions;
using ToDoApp.Shared.Extensions;

namespace ToDoApp.Server.Extensions;

public static class WebApplicationBuilderExtensions
{
	internal static WebApplicationBuilder RegisterPresentation(this WebApplicationBuilder builder)
	{
		builder.Services
			.RegisterApplicationServices(builder.Configuration)
			.RegisterApplicationRepositories(builder.Configuration)
			.RegisterValidationServices(builder.Configuration);

		return builder;
	}
}