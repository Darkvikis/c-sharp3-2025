namespace ToDoList.Test;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;

public class TodoListControllerCreateTests : TodoListControllerTestBase
{
    [Fact]
    public void CreateWithValidDtoReturnsCreatedResponse()
    {
        // Arrange
        var dto = CreateValidDto();

        // Act
        var result = Controller.Create(dto);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var returnValue = Assert.IsType<ToDoItemResponseDto>(createdResult.Value);

        Assert.Equal(dto.Name, returnValue.Name);
        Assert.Equal(dto.Description, returnValue.Description);
        Assert.Equal(dto.IsCompleted, returnValue.IsCompleted);
        Assert.True(returnValue.Id > 0);
    }

    [Fact]
    public void CreateWithNullDtoReturnsBadRequest()
    {
        // Act
        var result = Controller.Create(null!);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("Body is required.", badRequestResult.Value);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("\t")]
    [InlineData("\n")]
    [InlineData("  \t  \n  ")]
    public void CreateWithInvalidNameReturnsBadRequest(string invalidName)
    {
        // Arrange
        var dto = CreateValidDto(name: invalidName);

        // Act
        var result = Controller.Create(dto);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("Name is required.", badRequestResult.Value);
    }

    [Fact]
    public void CreateAssignsUniqueId()
    {
        // Arrange
        var dto1 = CreateValidDto("Task 1");
        var dto2 = CreateValidDto("Task 2");

        // Act
        var result1 = Controller.Create(dto1);
        var result2 = Controller.Create(dto2);

        // Assert
        var createdResult1 = Assert.IsType<CreatedAtActionResult>(result1.Result);
        var createdResult2 = Assert.IsType<CreatedAtActionResult>(result2.Result);
        var returnValue1 = Assert.IsType<ToDoItemResponseDto>(createdResult1.Value);
        var returnValue2 = Assert.IsType<ToDoItemResponseDto>(createdResult2.Value);

        Assert.NotEqual(returnValue1.Id, returnValue2.Id);
    }
}
