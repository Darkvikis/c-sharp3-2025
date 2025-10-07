namespace ToDoList.Domain.Models;

public class ToDoItem
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public bool IsCompleted { get; set; }
}
