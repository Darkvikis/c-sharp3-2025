namespace ToDoList.Test.UnitTests;

using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.Persistence.Repositories;
using ToDoList.WebApi;

public class GetTests
{
    private static (TodoListController controller, IRepository<ToDoItem> repo) CreateController()
    {
        var repo = Substitute.For<IRepository<ToDoItem>>();
        return (new TodoListController(repo), repo);
    }

    [Fact]
    public void ReadReturnsEmptyList()
    {
        var (controller, repo) = CreateController();
        repo.ReadAll().Returns([]);

        var result = controller.Read();

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var items = Assert.IsAssignableFrom<IEnumerable<ToDoItemResponseDto>>(ok.Value);
        Assert.Empty(items);
        repo.Received(1).ReadAll();
    }

    [Fact]
    public void ReadReturnsMappedItems()
    {
        var (controller, repo) = CreateController();
        var entities = new List<ToDoItem>
        {
            new() { Id = 1, Name = "A", Description = "Desc A", IsCompleted = false },
            new() { Id = 2, Name = "B", Description = "Desc B", IsCompleted = true }
        };
        repo.ReadAll().Returns(entities);

        var result = controller.Read();

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var dtos = Assert.IsAssignableFrom<IEnumerable<ToDoItemResponseDto>>(ok.Value).ToList();
        Assert.Equal(2, dtos.Count);
        Assert.Collection(dtos,
            d =>
            {
                Assert.Equal(1, d.Id);
                Assert.Equal("A", d.Name);
                Assert.Equal("Desc A", d.Description);
                Assert.False(d.IsCompleted);
            },
            d =>
            {
                Assert.Equal(2, d.Id);
                Assert.Equal("B", d.Name);
                Assert.Equal("Desc B", d.Description);
                Assert.True(d.IsCompleted);
            });
        repo.Received(1).ReadAll();
    }

    [Fact]
    public void ReadByIdReturnsNotFoundWhenMissing()
    {
        var (controller, repo) = CreateController();
        repo.Read(10).Returns((ToDoItem?)null);

        var result = controller.ReadById(10);

        Assert.IsType<NotFoundResult>(result.Result);
        repo.Received(1).Read(10);
    }

    [Fact]
    public void ReadByIdReturnsItem()
    {
        var (controller, repo) = CreateController();
        var entity = new ToDoItem { Id = 5, Name = "Task", Description = "Do it", IsCompleted = true };
        repo.Read(5).Returns(entity);

        var result = controller.ReadById(5);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var dto = Assert.IsType<ToDoItemResponseDto>(ok.Value);
        Assert.Equal(entity.Id, dto.Id);
        Assert.Equal(entity.Name, dto.Name);
        Assert.Equal(entity.Description, dto.Description);
        Assert.Equal(entity.IsCompleted, dto.IsCompleted);
        repo.Received(1).Read(5);
    }
}
