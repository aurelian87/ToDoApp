using Microsoft.EntityFrameworkCore;
using ToDoApp.Shared.Models;

namespace ToDoApp.PersistenceLayer.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options) {}

        public DbSet<ToDoModel> ToDos { get; set; }
    }
}