namespace ToDoList.Test;

using Microsoft.EntityFrameworkCore;
using ToDoList.WebApi;
using ToDoList.Domain.DTOs;
using ToDoList.Persistence;
using ToDoList.Domain.Models;
using ToDoList.Persistence.Repositories;

/// <summary>
/// Base class for TodoListController tests with shared helper methods
/// </summary>
public abstract class TodoListControllerTestBase
{
    protected TodoListController Controller { get; set; }

    protected ToDoItemsContext Context { get; set; }

    protected IRepository<ToDoItem> Repository { get; set; }

    public TodoListControllerTestBase()
    {
        string dbDir = Path.Combine(AppContext.BaseDirectory, "TestData");
        Directory.CreateDirectory(dbDir);
        string dbFilePath = Path.Combine(dbDir, "localdb_test.db");
        string connectionString = $"Data Source={dbFilePath}";

        Context = new ToDoItemsContext(connectionString);
        Repository = new TestToDoItemRepository(Context);
        Controller = new TodoListController(Repository);
    }

    protected void PrepareTest()
    {
        var toRemove = Context.ToDoItems.ToList();
        Context.ToDoItems.RemoveRange(toRemove);
        Context.SaveChanges();

        var toDoItem = new ToDoItem
        {
            Id = 1,
            Name = "Jmeno",
            Description = "Popis",
            IsCompleted = false
        };

        Context.ToDoItems.Add(toDoItem);
        Context.SaveChanges();
    }

    protected static ToDoItemCreateRequestDto CreateValidDto(
        string name = "Test Task",
        string description = "Test Description",
        bool isCompleted = false) => new(name, description, isCompleted);

    protected static ToDoItemUpdateRequestDto CreateValidUpdateDto(
        int id,
        string name = "Updated Task",
        string description = "Updated Description",
        bool isCompleted = true) => new(id, name, description, isCompleted);

    private sealed class TestToDoItemRepository(ToDoItemsContext ctx) : IRepository<ToDoItem>
    {
        private readonly ToDoItemsContext ctx = ctx;

        public void Create(ToDoItem entity)
        {
            ctx.ToDoItems.Add(entity);
            ctx.SaveChanges();
        }

        public IEnumerable<ToDoItem> ReadAll()
            => [.. ctx.ToDoItems.AsNoTracking()];

        public ToDoItem? Read(int id)
            => ctx.ToDoItems.AsNoTracking().FirstOrDefault(x => x.Id == id);

        public void Update(ToDoItem entity)
        {
            ctx.ToDoItems.Update(entity);
            ctx.SaveChanges();
        }

        public void Delete(int id)
        {
            var found = ctx.ToDoItems.Find(id);
            if (found is null)
            {
                return;
            }

            ctx.ToDoItems.Remove(found);
            ctx.SaveChanges();
        }
    }
}
