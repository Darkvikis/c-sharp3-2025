namespace ToDoList.Domain.DTOs;

public record ToDoItemResponseDto(
    int Id,
    string Name,
    string Description,
    bool IsCompleted
);
