namespace ToDoList.WebApi;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;

[ApiController]
[Route("api/todo-items")]
public class TodoListController : ControllerBase
{
    private static readonly List<ToDoItem> items =
    [
        new() { Id = 1, Name = "Buy groceries",     Description = "Milk, eggs, bread, and vegetables.", IsCompleted = false },
        new() { Id = 2, Name = "Finish C# project", Description = "Implement the ToDoList API and write unit tests.", IsCompleted = false },
        new() { Id = 3, Name = "Call mom",          Description = "Weekly check-in call.", IsCompleted = true }
    ];

    [HttpPost]
    public ActionResult<ToDoItemResponseDto> Create([FromBody] ToDoItemCreateRequestDto dto)
    {
        if (dto is null)
        {
            return BadRequest("Body is required.");
        }
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            return BadRequest("Name is required.");
        }

        var used = items.Select(x => x.Id).OrderBy(x => x).ToList();
        int newId = 1;
        foreach (int id in used)
        {
            if (id != newId)
            {
                break;
            }
            newId++;
        }

        var entity = new ToDoItem
        {
            Id = newId,
            Name = dto.Name,
            Description = dto.Description,
            IsCompleted = dto.IsCompleted
        };

        items.Add(entity);
        var result = MapToResponse(entity);
        return CreatedAtAction(nameof(ReadById), new { id = result.Id }, result);
    }

    [HttpGet]
    public ActionResult<IEnumerable<ToDoItemResponseDto>> Read()
        => Ok(items.Select(MapToResponse));

    [HttpGet("{id:int}")]
    public ActionResult<ToDoItemResponseDto> ReadById([FromRoute] int id)
    {
        var item = items.FirstOrDefault(x => x.Id == id);
        return item is null ? NotFound() : Ok(MapToResponse(item));
    }

    [HttpPut("{id:int}")]
    public IActionResult UpdateById([FromRoute] int id, [FromBody] ToDoItemUpdateRequestDto dto)
    {
        if (dto is null)
        {
            return BadRequest("Body is required.");
        }
        if (id != dto.Id)
        {
            return BadRequest("Route id must match body id.");
        }
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            return BadRequest("Name is required.");
        }

        var existing = items.FirstOrDefault(x => x.Id == id);
        if (existing is null)
        {
            return NotFound();
        }

        existing.Name = dto.Name;
        existing.Description = dto.Description;
        existing.IsCompleted = dto.IsCompleted;

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public IActionResult DeleteById([FromRoute] int id)
    {
        int idx = items.FindIndex(x => x.Id == id);
        if (idx < 0)
        {
            return NotFound();
        }
        items.RemoveAt(idx);
        return NoContent();
    }

    private static ToDoItemResponseDto MapToResponse(ToDoItem x)
        => new(x.Id, x.Name, x.Description, x.IsCompleted);
}
