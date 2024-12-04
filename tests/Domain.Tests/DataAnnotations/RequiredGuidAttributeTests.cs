using Core.Domain.DataAnnotations;

namespace Domain.Tests.DataAnnotations;

public class RequiredGuidAttributeTests
{
    [Fact]
    public void IsValid_ShouldReturnTrue_ForValidGuid()
    {
        // Arrange
        var attribute = new RequiredGuidAttribute();
        var validGuid = Guid.NewGuid();

        // Act
        var result = attribute.IsValid(validGuid);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValid_ShouldReturnFalse_ForEmptyGuid()
    {
        // Arrange
        var attribute = new RequiredGuidAttribute();
        var emptyGuid = Guid.Empty;

        // Act
        var result = attribute.IsValid(emptyGuid);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValid_ShouldReturnFalse_ForNullValue()
    {
        // Arrange
        var attribute = new RequiredGuidAttribute();

        // Act
        var result = attribute.IsValid(null);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValid_ShouldReturnFalse_ForNonGuidValue()
    {
        // Arrange
        var attribute = new RequiredGuidAttribute();
        var nonGuidValue = "not-a-guid";

        // Act
        var result = attribute.IsValid(nonGuidValue);

        // Assert
        Assert.False(result);
    }
}