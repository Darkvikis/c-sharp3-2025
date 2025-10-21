namespace ToDoList.Persistence;

using Microsoft.EntityFrameworkCore;
using ToDoList.Domain.Models;

public class ToDoItemsContext : DbContext
{
    private readonly string connectionString;

    public DbSet<ToDoItem> ToDoItems { get; set; } = null!;

    public ToDoItemsContext(string connectionString = "DataSource=../../data/localdb.db")
    {
        this.connectionString = connectionString;
        this.Database.Migrate();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(this.connectionString);
    }
}
