using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ToDoApp.Shared.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection RegisterValidationServices(this IServiceCollection services,
		IConfiguration config)
	{
		services.AddValidatorsWithNamingConvention(Assembly.GetExecutingAssembly());

		return services;
	}

	public static void AddValidatorsWithNamingConvention(this IServiceCollection services, Assembly assembly)
	{
		// Find all classes that end with "ModelValidator" and implement IValidator<>
		var validatorTypes = assembly.GetTypes()
			.Where(t => !t.IsAbstract && t.Name.EndsWith("ModelValidator") &&
						t.GetInterfaces().Any(i =>
							i.IsGenericType &&
							i.GetGenericTypeDefinition() == typeof(IValidator<>)));

		foreach (var validatorType in validatorTypes)
		{
			// Get the implemented IValidator<T> interface
			var modelType = validatorType.GetInterfaces()
				.FirstOrDefault(i =>
					i.IsGenericType &&
					i.GetGenericTypeDefinition() == typeof(IValidator<>))?
				.GetGenericArguments()[0];

			if (modelType is not null)
			{
				// Register the validator as scoped
				services.AddScoped(typeof(IValidator<>).MakeGenericType(modelType), validatorType);
			}
		}
	}
}