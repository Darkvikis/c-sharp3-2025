namespace ToDoList.Domain.DTOs;

public record ToDoItemListItemDto(
    int Id,
    string Name,
    bool IsCompleted
);
