using Amazon.Extensions.NETCore.Setup;
using Amazon.SQS;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.Tests.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static void AddAwsSqsMessageBroker(this IServiceCollection services)
    {
        if (!services.Any(service => service.ServiceType == typeof(AWSOptions)))
        {
            var awsOptions = new AWSOptions
            {
                Profile = "default",
                Region = Amazon.RegionEndpoint.USEast1
            };

            services.AddDefaultAWSOptions(awsOptions);
        }

        if (!services.Any(service => service.ServiceType == typeof(IAmazonSQS)))
        {
            services.AddAWSService<IAmazonSQS>();
        }
    }
}

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddAwsSqsMessageBroker_ShouldRegisterAwsSqsServices()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddAwsSqsMessageBroker();
        var serviceProvider = services.BuildServiceProvider();

        // Assert
        var awsOptions = serviceProvider.GetService<AWSOptions>();
        Assert.NotNull(awsOptions);
        Assert.Equal("default", awsOptions.Profile);
        Assert.Equal(Amazon.RegionEndpoint.USEast1, awsOptions.Region);

        var sqsClient = serviceProvider.GetService<IAmazonSQS>();
        Assert.NotNull(sqsClient);
    }

    [Fact]
    public void AddAwsSqsMessageBroker_ShouldThrowArgumentNullException_WhenServiceCollectionIsNull()
    {
        // Arrange
        ServiceCollection services = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => services.AddAwsSqsMessageBroker());
    }

    [Fact]
    public void AddAwsSqsMessageBroker_ShouldRegisterAwsSqsServices_WithCustomAWSOptions()
    {
        // Arrange
        var services = new ServiceCollection();
        var customAwsOptions = new AWSOptions
        {
            Profile = "custom-profile",
            Region = Amazon.RegionEndpoint.USWest2
        };

        // Act
        services.AddDefaultAWSOptions(customAwsOptions);
        services.AddAwsSqsMessageBroker();
        var serviceProvider = services.BuildServiceProvider();

        // Assert
        var awsOptions = serviceProvider.GetService<AWSOptions>();
        Assert.NotNull(awsOptions);
        Assert.Equal("custom-profile", awsOptions.Profile);
        Assert.Equal(Amazon.RegionEndpoint.USWest2, awsOptions.Region);

        var sqsClient = serviceProvider.GetService<IAmazonSQS>();
        Assert.NotNull(sqsClient);
    }

    [Fact]
    public void AddAwsSqsMessageBroker_Should_ConfigureAwsSqsServices()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddAwsSqsMessageBroker();
        var serviceProvider = services.BuildServiceProvider();

        // Assert
        var awsOptions = serviceProvider.GetService<AWSOptions>();
        Assert.NotNull(awsOptions);
        Assert.Equal("default", awsOptions.Profile);
        Assert.Equal(Amazon.RegionEndpoint.USEast1, awsOptions.Region);

        var sqsClient = serviceProvider.GetService<IAmazonSQS>();
        Assert.NotNull(sqsClient);
    }
}