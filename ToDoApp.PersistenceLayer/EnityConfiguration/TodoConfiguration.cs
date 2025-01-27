using ToDoApp.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace ToDoApp.PersistenceLayer.EnityConfiguration;

public static class TodoConfiguration
{
	public static void Configure(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<TodoModel>()
				.HasKey(e => e.Id);

		modelBuilder.Entity<TodoModel>()
			.Property(e => e.Id)
			.UseIdentityColumn(1, 1);

		modelBuilder.Entity<TodoModel>()
			.HasOne<UserProfileModel>()
			.WithMany()
			.HasForeignKey(e => e.UserProfileId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}