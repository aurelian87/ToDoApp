using Microsoft.EntityFrameworkCore;
using ToDoApp.Shared.Models;

namespace ToDoApp.PersistenceLayer.EnityConfiguration;

public static class UserProfileConfiguration
{
	public static void Configure(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<UserProfileModel>()
				.HasKey(e => e.Id);

		modelBuilder.Entity<UserProfileModel>()
			.Property(e => e.Id)
			.UseIdentityColumn(1, 1);
	}
}