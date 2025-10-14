namespace ToDoList.Test;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;

public class TodoListControllerReadTests : TodoListControllerTestBase
{
    [Fact]
    public void ReadReturnsAllItems()
    {
        // Act
        var result = Controller.Read();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var items = Assert.IsAssignableFrom<IEnumerable<ToDoItemResponseDto>>(okResult.Value);

        // Should contain at least the 3 default items
        Assert.True(items.Count() >= 3);
    }


    [Theory]
    [InlineData(1, true)]  // Existing ID
    [InlineData(2, true)]  // Existing ID
    [InlineData(3, true)]  // Existing ID
    [InlineData(999, false)] // Non-existing ID
    [InlineData(-1, false)]  // Invalid ID
    [InlineData(0, false)]   // Invalid ID
    public void ReadByIdReturnsCorrectResult(int id, bool shouldExist)
    {
        // Act
        var result = Controller.ReadById(id);

        // Assert
        if (shouldExist)
        {
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var item = Assert.IsType<ToDoItemResponseDto>(okResult.Value);
            Assert.Equal(id, item.Id);
        }
        else
        {
            Assert.IsType<NotFoundResult>(result.Result);
        }
    }
}
