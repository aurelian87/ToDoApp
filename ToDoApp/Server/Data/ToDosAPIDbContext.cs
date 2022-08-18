using Microsoft.EntityFrameworkCore;
using ToDoApp.Shared.Models;

namespace ToDoApp.Server.Data
{
    public class ToDosAPIDbContext : DbContext
    {
        public ToDosAPIDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<ToDoModel> ToDos { get; set; }
    }
}