using Microsoft.EntityFrameworkCore;
using ToDoApp.PersistenceLayer.EnityConfiguration;
using ToDoApp.Shared.Models;

namespace ToDoApp.PersistenceLayer.Data;

public class DatabaseContext : DbContext
{
	public DatabaseContext(DbContextOptions options) : base(options) { }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);
		UserCredentialConfiguration.Configure(modelBuilder);
		UserProfileConfiguration.Configure(modelBuilder);
		TodoConfiguration.Configure(modelBuilder);
	}

	public DbSet<UserCredentialModel> UserCredential { get; set; }

	public DbSet<UserProfileModel> UserProfile { get; set; }

	public DbSet<TodoModel> Todo { get; set; }
}