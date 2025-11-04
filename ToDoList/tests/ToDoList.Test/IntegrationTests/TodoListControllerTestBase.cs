namespace ToDoList.Test;

using ToDoList.WebApi;
using ToDoList.Domain.DTOs;
using ToDoList.Persistence;
using ToDoList.Domain.Models;

/// <summary>
/// Base class for TodoListController tests with shared helper methods
/// </summary>
public abstract class TodoListControllerTestBase
{
    protected TodoListController Controller { get; set; }

    protected ToDoItemsContext Context { get; set; }

    public TodoListControllerTestBase()
    {
        // Resolve stable absolute path under test output
        var dbDir = Path.Combine(AppContext.BaseDirectory, "TestData");
        Directory.CreateDirectory(dbDir); // Ensure folder exists

        var dbFilePath = Path.Combine(dbDir, "localdb_test.db");
        var connectionString = $"Data Source={dbFilePath}";

        Context = new ToDoItemsContext(connectionString);
        Controller = new TodoListController(Context);
    }

    protected void PrepareTest()
    {
        var toRemove = Context.ToDoItems.ToList();
        Context.ToDoItems.RemoveRange(toRemove);

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
}
