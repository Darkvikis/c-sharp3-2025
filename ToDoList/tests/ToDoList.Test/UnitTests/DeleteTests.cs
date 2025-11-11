namespace ToDoList.Test.UnitTests;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using ToDoList.Domain.Models;

public class DeleteTests : BaseUnitTests
{
    [Fact]
    public void DeleteReturnsNotFoundWhenMissing()
    {
        //
        var (controller, repo) = CreateController();
        repo.Read(9).Returns((ToDoItem?)null);

        //Act
        var result = controller.DeleteById(9);

        //Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void DeleteRemovesEntityAndReturnsNoContent()
    {
        //
        var (controller, repo) = CreateController();
        ToDoItem entity = new() { Id = 9, Name = "Test", Description = "Something", IsCompleted = true };
        repo.Read(9).Returns((ToDoItem?)entity);

        //Act
        var result = controller.DeleteById(9);

        //Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public void DeleteWhenReadThrowsReturnsInternalServerError()
    {
        var (controller, repo) = CreateController();

        repo.When(r => r.Read(42))
            .Do(_ => throw new Exception("DB down"));

        var result = controller.DeleteById(42);

        var obj = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, obj.StatusCode);
    }

    [Fact]
    public void DeleteWhenDeleteThrowsReturnsInternalServerError()
    {
        var (controller, repo) = CreateController();

        ToDoItem entity = new() { Id = 42, Name = "Test", Description = "Something", IsCompleted = true };
        repo.Read(42).Returns((ToDoItem?)entity);

        repo.When(r => r.Delete(42))
            .Do(_ => throw new Exception("DB down"));

        var result = controller.DeleteById(42);

        var obj = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, obj.StatusCode);
        repo.Received(1).Read(42);
        repo.Received(1).Delete(42);
    }
}
