
namespace ToDoList.Test.UnitTests;

using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.WebApi;

public class PostTests : BaseUnitTests
{
    [Fact]
    public void CreateReturnsBadRequestWhenDtoIsNull()
    {
        var (controller, repo) = CreateController();

        var result = controller.Create(null);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("Body is required.", badRequest.Value);
        repo.DidNotReceive().Create(Arg.Any<ToDoItem>());
    }

    [Fact]
    public void CreateReturnsBadRequestWhenNameIsWhitespace()
    {
        var (controller, repo) = CreateController();
        var dto = new ToDoItemCreateRequestDto(" ", "Desc", true);

        var result = controller.Create(dto);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("Name is required.", badRequest.Value);
        repo.DidNotReceive().Create(Arg.Any<ToDoItem>());
    }

    [Fact]
    public void CreatePersistsEntityAndReturnsCreated()
    {
        var (controller, repo) = CreateController();
        var dto = new ToDoItemCreateRequestDto("Task A", "Do something", true);

        repo
            .When(r => r.Create(Arg.Any<ToDoItem>()))
            .Do(call =>
            {
                var entity = call.Arg<ToDoItem>();
                entity.Id = 123;
            });

        var result = controller.Create(dto);

        var created = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(nameof(TodoListController.ReadById), created.ActionName);
        Assert.True(created.RouteValues?.ContainsKey("id"));
        Assert.Equal(123, created.RouteValues?["id"]);

        var response = Assert.IsType<ToDoItemResponseDto>(created.Value);
        Assert.Equal(123, response.Id);
        Assert.Equal(dto.Name, response.Name);
        Assert.Equal(dto.Description, response.Description);
        Assert.Equal(dto.IsCompleted, response.IsCompleted);

        repo.Received(1).Create(Arg.Is<ToDoItem>(x =>
            x.Name == dto.Name &&
            x.Description == dto.Description &&
            x.IsCompleted == dto.IsCompleted));
    }

    [Fact]
    public void CreateUsesDefaultIdWhenRepositoryDoesNotAssign()
    {
        var (controller, repo) = CreateController();
        var dto = new ToDoItemCreateRequestDto("Task B", "Another task", false);

        var result = controller.Create(dto);

        var created = Assert.IsType<CreatedAtActionResult>(result.Result);
        var response = Assert.IsType<ToDoItemResponseDto>(created.Value);
        Assert.Equal(0, response.Id); // default since mock did not set
        Assert.Equal(dto.Name, response.Name);
        Assert.Equal(dto.Description, response.Description);
        Assert.Equal(dto.IsCompleted, response.IsCompleted);

        repo.Received(1).Create(Arg.Is<ToDoItem>(x =>
            x.Name == dto.Name &&
            x.Description == dto.Description &&
            x.IsCompleted == dto.IsCompleted &&
            x.Id == 0));
    }
}
