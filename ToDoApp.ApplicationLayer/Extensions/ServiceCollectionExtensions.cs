
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ToDoApp.ApplicationLayer.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection RegisterApplicationServices(this IServiceCollection services,
		IConfiguration config)
	{
		services.AddServices(Assembly.GetExecutingAssembly());

		return services;
	}

	public static void AddServices(this IServiceCollection services, Assembly assembly)
	{
		// Find all types implementing an interface named I[TypeName]Service and the class is not abstract
		var serviceTypes = assembly.GetTypes()
			.Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces().Any(i => i.Name == $"I{t.Name}"));

		foreach (var serviceType in serviceTypes)
		{
			var interfaceType = serviceType.GetInterfaces().FirstOrDefault(i => i.Name == $"I{serviceType.Name}");
			if (interfaceType != null)
			{
				services.AddScoped(interfaceType, serviceType);
			}
		}
	}
}