using Amazon.CognitoIdentityProvider;
using Core.Domain.Notificacoes;
using Core.WebApi.Configurations;
using Core.WebApi.DependencyInjection;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Api.Tests.Configurations;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddApiDefautConfig_Should_ConfigureServices()
    {
        // Arrange
        var services = new ServiceCollection();
        var jwtBearerConfigureOptions = new JwtBearerConfigureOptions
        {
            Authority = "https://example.com",
            MetadataAddress = "https://example.com/.well-known/openid-configuration"
        };

        // Act
        services.AddApiDefautConfig(jwtBearerConfigureOptions);
        var serviceProvider = services.BuildServiceProvider();

        // Assert
        var notificador = serviceProvider.GetService<INotificador>();
        Assert.NotNull(notificador);

        var jsonOptions = serviceProvider.GetService<IOptions<JsonOptions>>();
        Assert.NotNull(jsonOptions);
        Assert.Equal(JsonIgnoreCondition.WhenWritingNull, jsonOptions.Value.JsonSerializerOptions.DefaultIgnoreCondition);
        Assert.Equal(JsonNamingPolicy.CamelCase, jsonOptions.Value.JsonSerializerOptions.PropertyNamingPolicy);

        var authenticationSchemeProvider = serviceProvider.GetService<IAuthenticationSchemeProvider>();
        Assert.NotNull(authenticationSchemeProvider);
        var jwtBearerOptions = serviceProvider.GetService<IOptionsSnapshot<JwtBearerOptions>>().Get(JwtBearerDefaults.AuthenticationScheme);
        Assert.Equal(jwtBearerConfigureOptions.Authority, jwtBearerOptions.Authority);
        Assert.Equal(jwtBearerConfigureOptions.MetadataAddress, jwtBearerOptions.MetadataAddress);
        Assert.True(jwtBearerOptions.IncludeErrorDetails);
        Assert.False(jwtBearerOptions.RequireHttpsMetadata);
        Assert.True(jwtBearerOptions.TokenValidationParameters.ValidateIssuer);
        Assert.False(jwtBearerOptions.TokenValidationParameters.ValidateAudience);
        Assert.True(jwtBearerOptions.TokenValidationParameters.ValidateIssuerSigningKey);
        Assert.Equal("cognito:groups", jwtBearerOptions.TokenValidationParameters.RoleClaimType);

        var authorizationOptions = serviceProvider.GetService<IOptions<AuthorizationOptions>>();
        Assert.NotNull(authorizationOptions);
        Assert.Contains(authorizationOptions.Value.GetPolicy("AdminRole").Requirements, r => r is RolesAuthorizationRequirement);
        Assert.Contains(authorizationOptions.Value.GetPolicy("ClienteRole").Requirements, r => r is RolesAuthorizationRequirement);
        Assert.Contains(authorizationOptions.Value.GetPolicy("AdminOrClienteRole").Requirements, r => r is RolesAuthorizationRequirement);

        var cognitoService = serviceProvider.GetService<IAmazonCognitoIdentityProvider>();
        Assert.NotNull(cognitoService);
    }
}