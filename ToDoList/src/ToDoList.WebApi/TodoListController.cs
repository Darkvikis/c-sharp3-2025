namespace ToDoList.WebApi;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.Persistence.Repositories;

[ApiController]
[Route("api/todo-items")]
public class TodoListController(IRepository<ToDoItem> repository) : ControllerBase
{
    private readonly IRepository<ToDoItem> Repository = repository;


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

        var entity = new ToDoItem
        {
            Name = dto.Name,
            Description = dto.Description,
            IsCompleted = dto.IsCompleted
        };

        Repository.Create(entity);
        var result = MapToResponse(entity);
        return CreatedAtAction(nameof(ReadById), new { id = result.Id }, result);
    }

    [HttpGet]
    public ActionResult<IEnumerable<ToDoItemResponseDto>> Read()
        => Ok(Repository.ReadAll().Select(MapToResponse));

    [HttpGet("{id:int}")]
    public ActionResult<ToDoItemResponseDto> ReadById([FromRoute] int id)
    {
        var item = Repository.Read(id);
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

        var existing = Repository.Read(id);
        if (existing is null)
        {
            return NotFound();
        }

        existing.Name = dto.Name;
        existing.Description = dto.Description;
        existing.IsCompleted = dto.IsCompleted;

        Repository.Update(existing);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public IActionResult DeleteById([FromRoute] int id)
    {
        var entity = Repository.Read(id);
        if (entity is null)
        {
            return NotFound();
        }
        Repository.Delete(id);
        return NoContent();
    }

    private static ToDoItemResponseDto MapToResponse(ToDoItem x)
        => new(x.Id, x.Name, x.Description, x.IsCompleted);
}
