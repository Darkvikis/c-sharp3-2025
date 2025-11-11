namespace ToDoList.Test.UnitTests;

using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;

public class PutTests : BaseUnitTests
{
    [Fact]
    public void UpdateReturnsBadRequestWhenDtoIsNull()
    {
        var (controller, repo) = CreateController();

        var result = controller.UpdateById(1, null);

        var bad = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Body is required.", bad.Value);
        repo.DidNotReceive().Update(Arg.Any<ToDoItem>());
    }

    [Fact]
    public void UpdateReturnsBadRequestWhenRouteIdMismatch()
    {
        var (controller, repo) = CreateController();
        var dto = new ToDoItemUpdateRequestDto(2, "Name", "Desc", false);

        var result = controller.UpdateById(1, dto);

        var bad = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Route id must match body id.", bad.Value);
        repo.DidNotReceive().Update(Arg.Any<ToDoItem>());
    }

    [Fact]
    public void UpdateReturnsBadRequestWhenNameIsWhitespace()
    {
        var (controller, repo) = CreateController();
        var dto = new ToDoItemUpdateRequestDto(1, "  ", "Desc", true);

        var result = controller.UpdateById(1, dto);

        var bad = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Name is required.", bad.Value);
        repo.DidNotReceive().Update(Arg.Any<ToDoItem>());
    }

    [Fact]
    public void UpdateReturnsNotFoundWhenEntityMissing()
    {
        var (controller, repo) = CreateController();
        var dto = new ToDoItemUpdateRequestDto(7, "Task", "Desc", false);
        repo.Read(7).Returns((ToDoItem?)null);

        var result = controller.UpdateById(7, dto);

        Assert.IsType<NotFoundResult>(result);
        repo.Received(1).Read(7);
        repo.DidNotReceive().Update(Arg.Any<ToDoItem>());
    }

    [Fact]
    public void UpdatePersistsChangesAndReturnsNoContent()
    {
        var (controller, repo) = CreateController();
        var existing = new ToDoItem { Id = 3, Name = "Old", Description = "Old Desc", IsCompleted = false };
        repo.Read(3).Returns(existing);
        var dto = new ToDoItemUpdateRequestDto(3, "New", "New Desc", true);

        var result = controller.UpdateById(3, dto);

        Assert.IsType<NoContentResult>(result);
        Assert.Equal("New", existing.Name);
        Assert.Equal("New Desc", existing.Description);
        Assert.True(existing.IsCompleted);
        repo.Received(1).Update(Arg.Is<ToDoItem>(x =>
            x.Id == 3 &&
            x.Name == "New" &&
            x.Description == "New Desc" &&
            x.IsCompleted));
    }
}
