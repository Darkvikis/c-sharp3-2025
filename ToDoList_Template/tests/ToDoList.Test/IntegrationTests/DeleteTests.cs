namespace ToDoList.Test.IntegrationTests;

public class DeleteTests
{
    [Fact]
    public void DeleteValidIdReturnsNoContent()
    {
        // Arrange
        string connectionString = "Data Source=../../../IntegrationTests/data/localdb_test.db";
        using var context = new ToDoItemsContext(connectionString);
        var repository = new ToDoItemsRepository(context);
        var controller = new ToDoItemsController(repository);

        var toDoItem = new ToDoItem
        {
            Name = "Jmeno",
            Description = "Popis",
            IsCompleted = false
        };
        context.ToDoItems.Add(toDoItem);
        context.SaveChanges();

        // Act
        var result = controller.DeleteById(toDoItem.ToDoItemId);

        // Assert
        Assert.IsType<NoContentResult>(result);

        // Verify item was deleted
        var deletedItem = context.ToDoItems.Find(toDoItem.ToDoItemId);
        Assert.Null(deletedItem);
    }

    [Fact]
    public void DeleteInvalidIdReturnsNotFound()
    {
        // Arrange
        string connectionString = "Data Source=../../../IntegrationTests/data/localdb_test.db";
        using var context = new ToDoItemsContext(connectionString);
        var repository = new ToDoItemsRepository(context);
        var controller = new ToDoItemsController(repository);

        // Act
        int invalidId = -1;
        var result = controller.DeleteById(invalidId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}

