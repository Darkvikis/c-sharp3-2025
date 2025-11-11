namespace ToDoList.Test;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

public class TodoListControllerDeleteTests : TodoListControllerTestBase
{
    [Fact]
    public async Task DeleteByIdWithExistingIdReturnsNoContent()
    {
        // Arrange
        PrepareTest();

        // Verify the item exists
        var getBeforeDelete = Controller.ReadById(1);
        Assert.IsType<OkObjectResult>(getBeforeDelete.Result);

        // Act - Delete the item
        var deleteResult = Controller.DeleteById(1);

        // Assert - Delete should return NoContent
        Assert.IsType<NoContentResult>(deleteResult);

        // Verify the specific item was deleted
        var getDeletedItem = Controller.ReadById(1);
        Assert.IsType<NotFoundResult>(getDeletedItem.Result);
    }

    [Theory]
    [InlineData(999)]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(int.MaxValue)]
    public void DeleteByIdWithNonExistingIdReturnsNotFound(int nonExistingId)
    {
        // Arrange
        PrepareTest();

        // Act
        var result = Controller.DeleteById(nonExistingId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}
