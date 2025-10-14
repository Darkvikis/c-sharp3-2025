namespace ToDoList.Test;

using ToDoList.WebApi;
using ToDoList.Domain.DTOs;

/// <summary>
/// Base class for TodoListController tests with shared helper methods
/// </summary>
public abstract class TodoListControllerTestBase
{
    protected TodoListController Controller { get; } = new();

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
