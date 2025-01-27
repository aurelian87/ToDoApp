using Microsoft.EntityFrameworkCore;
using ToDoApp.Shared.Models;

namespace ToDoApp.PersistenceLayer.EnityConfiguration;

public static class UserCredentialConfiguration
{
	public static void Configure(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<UserCredentialModel>()
				.HasKey(e => e.Id);

		modelBuilder.Entity<UserCredentialModel>()
			.Property(e => e.Id)
			.UseIdentityColumn(1, 1);

		modelBuilder.Entity<UserCredentialModel>()
			.HasOne(e => e.UserProfile)
			.WithMany()
			.HasForeignKey(e => e.UserProfileId)
			.OnDelete(DeleteBehavior.Restrict);

		modelBuilder.Entity<UserCredentialModel>()
			.HasIndex(e => e.UserProfileId)
			.IsUnique();
	}
}