namespace ToDoList.Test.UnitTests;

using NSubstitute;
using ToDoList.Domain.Models;
using ToDoList.Persistence.Repositories;
using ToDoList.WebApi;

public abstract class BaseUnitTests
{
    protected static (TodoListController controller, IRepository<ToDoItem> repo) CreateController()
    {
        var repo = Substitute.For<IRepository<ToDoItem>>();
        return (new TodoListController(repo), repo);
    }
}
