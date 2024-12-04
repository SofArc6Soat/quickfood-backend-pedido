using Core.WebApi.Configurations;

namespace Api.Tests.Configurations;

public class JwtBearerConfigureOptionsTests
{
    [Fact]
    public void Should_Have_Default_Values()
    {
        // Arrange & Act
        var options = new JwtBearerConfigureOptions();

        // Assert
        Assert.Equal(string.Empty, options.Authority);
        Assert.Equal(string.Empty, options.MetadataAddress);
    }

    [Fact]
    public void Should_Set_Authority()
    {
        // Arrange
        var options = new JwtBearerConfigureOptions();
        var expectedAuthority = "https://example.com";

        // Act
        options.Authority = expectedAuthority;

        // Assert
        Assert.Equal(expectedAuthority, options.Authority);
    }

    [Fact]
    public void Should_Set_MetadataAddress()
    {
        // Arrange
        var options = new JwtBearerConfigureOptions();
        var expectedMetadataAddress = "https://example.com/.well-known/openid-configuration";

        // Act
        options.MetadataAddress = expectedMetadataAddress;

        // Assert
        Assert.Equal(expectedMetadataAddress, options.MetadataAddress);
    }

    [Fact]
    public void Should_Set_Authority_And_MetadataAddress()
    {
        // Arrange
        var options = new JwtBearerConfigureOptions();
        var expectedAuthority = "https://example.com";
        var expectedMetadataAddress = "https://example.com/.well-known/openid-configuration";

        // Act
        options.Authority = expectedAuthority;
        options.MetadataAddress = expectedMetadataAddress;

        // Assert
        Assert.Equal(expectedAuthority, options.Authority);
        Assert.Equal(expectedMetadataAddress, options.MetadataAddress);
    }
}