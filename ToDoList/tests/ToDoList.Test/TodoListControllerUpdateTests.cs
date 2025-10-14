namespace ToDoList.Test;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;

public class TodoListControllerUpdateTests : TodoListControllerTestBase
{
    [Fact]
    public void UpdateByIdWithValidDtoReturnsNoContent()
    {
        // Arrange
        var dto = CreateValidUpdateDto(1, "Updated Task", "Updated Description", true);

        // Act
        var result = Controller.UpdateById(1, dto);

        // Assert
        Assert.IsType<NoContentResult>(result);

        // Verify the item was actually updated
        var getResult = Controller.ReadById(1);
        var okResult = Assert.IsType<OkObjectResult>(getResult.Result);
        var updatedItem = Assert.IsType<ToDoItemResponseDto>(okResult.Value);

        Assert.Equal("Updated Task", updatedItem.Name);
        Assert.Equal("Updated Description", updatedItem.Description);
        Assert.True(updatedItem.IsCompleted);
    }

    [Fact]
    public void UpdateByIdWithNullDtoReturnsBadRequest()
    {
        // Act
        var result = Controller.UpdateById(1, null!);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Body is required.", badRequestResult.Value);
    }

    [Theory]
    [InlineData(1, 2, "Updated Task", "Route id must match body id.")]
    [InlineData(5, 3, "Another Task", "Route id must match body id.")]
    [InlineData(1, 1, "", "Name is required.")]
    [InlineData(1, 1, "   ", "Name is required.")]
    [InlineData(1, 1, "\t", "Name is required.")]
    public void UpdateByIdWithInvalidDataReturnsBadRequest(int routeId, int dtoId, string name, string expectedError)
    {
        // Arrange
        var dto = CreateValidUpdateDto(dtoId, name, "Description");

        // Act
        var result = Controller.UpdateById(routeId, dto);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(expectedError, badRequestResult.Value);
    }

    [Fact]
    public void UpdateByIdWithNonExistingIdReturnsNotFound()
    {
        // Arrange
        var dto = CreateValidUpdateDto(999, "Updated Task");

        // Act
        var result = Controller.UpdateById(999, dto);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}
