namespace ToDoList.WebApi;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.Persistence;

[ApiController]
[Route("api/todo-items")]
public class TodoListController(ToDoItemsContext dbContext) : ControllerBase
{
    private readonly ToDoItemsContext dbContext = dbContext;

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

        var used = dbContext.ToDoItems.Select(x => x.Id).OrderBy(x => x).ToList();
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

        dbContext.ToDoItems.Add(entity);
        dbContext.SaveChanges();
        var result = MapToResponse(entity);
        return CreatedAtAction(nameof(ReadById), new { id = result.Id }, result);
    }

    [HttpGet]
    public ActionResult<IEnumerable<ToDoItemResponseDto>> Read()
        => Ok(dbContext.ToDoItems.AsNoTracking().Select(MapToResponse));

    [HttpGet("{id:int}")]
    public ActionResult<ToDoItemResponseDto> ReadById([FromRoute] int id)
    {
        var item = dbContext.ToDoItems.AsNoTracking().FirstOrDefault(x => x.Id == id);
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

        var existing = dbContext.ToDoItems.FirstOrDefault(x => x.Id == id);
        if (existing is null)
        {
            return NotFound();
        }

        existing.Name = dto.Name;
        existing.Description = dto.Description;
        existing.IsCompleted = dto.IsCompleted;

        dbContext.SaveChanges();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public IActionResult DeleteById([FromRoute] int id)
    {
        var entity = dbContext.ToDoItems.FirstOrDefault(x => x.Id == id);
        if (entity is null)
        {
            return NotFound();
        }
        dbContext.ToDoItems.Remove(entity);
        dbContext.SaveChanges();
        return NoContent();
    }

    private static ToDoItemResponseDto MapToResponse(ToDoItem x)
        => new(x.Id, x.Name, x.Description, x.IsCompleted);
}
