namespace ToDoList.Test;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;

public class TodoListControllerUpdateTests : TodoListControllerTestBase
{
    [Fact]
    public void UpdateByIdWithNullDtoReturnsBadRequest()
    {
        // Arrange
        PrepareTest();

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
        PrepareTest();
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
        PrepareTest();
        var dto = CreateValidUpdateDto(999, "Updated Task");

        // Act
        var result = Controller.UpdateById(999, dto);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}
