namespace ToDoList.Domain.DTOs;

public record ToDoItemUpdateRequestDto(
    int Id,
    string Name,
    string Description,
    bool IsCompleted
);
