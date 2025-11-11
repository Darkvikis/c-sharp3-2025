namespace ToDoList.Test.UnitTests;

using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using ToDoList.Domain.Models;
using ToDoList.Persistence.Repositories;
using ToDoList.WebApi;

public class DeleteTests
{
    private static (TodoListController controller, IRepository<ToDoItem> repo) CreateController()
    {
        var repo = Substitute.For<IRepository<ToDoItem>>();
        return (new TodoListController(repo), repo);
    }

    [Fact]
    public void DeleteReturnsNotFoundWhenMissing()
    {
        var (controller, repo) = CreateController();
        repo.Read(9).Returns((ToDoItem?)null);

        var result = controller.DeleteById(9);

        Assert.IsType<NotFoundResult>(result);
        repo.Received(1).Read(9);
        repo.DidNotReceive().Delete(Arg.Any<int>());
    }

    [Fact]
    public void DeleteRemovesEntityAndReturnsNoContent()
    {
        var (controller, repo) = CreateController();
        var entity = new ToDoItem { Id = 4, Name = "Task", Description = "Desc", IsCompleted = false };
        repo.Read(4).Returns(entity);

        var result = controller.DeleteById(4);

        Assert.IsType<NoContentResult>(result);
        repo.Received(1).Delete(4);
    }
}
