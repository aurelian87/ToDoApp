using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace ToDoApp.PersistenceLayer.Extensions;

public static class RepositoryCollectionExtensions
{
	public static IServiceCollection RegisterApplicationRepositories(this IServiceCollection services,
		IConfiguration config)
	{
		services.AddAllServices(Assembly.GetExecutingAssembly());

		return services;
	}

	public static void AddAllServices(this IServiceCollection services, Assembly assembly)
	{
		// Find all types implementing an interface named I[TypeName] and ending with "Repository"
		var serviceTypes = assembly.GetTypes()
			.Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith("Repository"))
			.Select(t => new
			{
				Service = t.GetInterfaces().FirstOrDefault(i => i.Name == $"I{t.Name}"),
				Implementation = t
			})
			.Where(x => x.Service != null);

		foreach (var type in serviceTypes)
		{
			if (type.Service is not null)
			{
				services.TryAddTransient(type.Service, type.Implementation);
			}
		}
	}
}