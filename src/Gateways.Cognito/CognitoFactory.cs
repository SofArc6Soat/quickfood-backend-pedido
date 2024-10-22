using Amazon.CognitoIdentityProvider.Model;
using Amazon.Extensions.CognitoAuthentication;
using System.Security.Cryptography;
using System.Text;

namespace Gateways.Cognito
{
    public class CognitoFactory(string clientId, string clientSecret, string userPoolId) : ICognitoFactory
    {
        public SignUpRequest CreateSignUpRequest(string email, string password, string name, string? cpf = null)
        {
            var signUpRequest = new SignUpRequest
            {
                ClientId = clientId,
                SecretHash = CalcularSecretHash(clientId, clientSecret, email),
                Username = email,
                Password = password,
                UserAttributes =
                [
                    new AttributeType { Name = "email", Value = email },
                    new AttributeType { Name = "name", Value = name }
                ]
            };

            if (!string.IsNullOrEmpty(cpf))
            {
                signUpRequest.UserAttributes.Add(new AttributeType { Name = "custom:cpf", Value = cpf });
            }

            return signUpRequest;
        }

        public AdminAddUserToGroupRequest CreateAddUserToGroupRequest(string email, string groupName) =>
            new()
            {
                GroupName = groupName,
                Username = email,
                UserPoolId = userPoolId
            };

        public ConfirmSignUpRequest CreateConfirmSignUpRequest(string email, string confirmationCode) =>
            new()
            {
                ClientId = clientId,
                Username = email,
                ConfirmationCode = confirmationCode,
                SecretHash = CalcularSecretHash(clientId, clientSecret, email)
            };

        public ForgotPasswordRequest CreateForgotPasswordRequest(string email) =>
            new()
            {
                ClientId = clientId,
                Username = email,
                SecretHash = CalcularSecretHash(clientId, clientSecret, email)
            };

        public ConfirmForgotPasswordRequest CreateConfirmForgotPasswordRequest(string email, string confirmationCode, string newPassword) =>
            new()
            {
                ClientId = clientId,
                Username = email,
                ConfirmationCode = confirmationCode,
                Password = newPassword,
                SecretHash = CalcularSecretHash(clientId, clientSecret, email)
            };

        public ListUsersRequest CreateListUsersRequestByEmail(string userPoolId, string email) =>
            new()
            {
                UserPoolId = userPoolId,
                Filter = $"email = \"{email}\""
            };

        public ListUsersRequest CreateListUsersRequestByAll(string userPoolId, string? paginationToken) =>
            new()
            {
                UserPoolId = userPoolId,
                PaginationToken = paginationToken
            };

        public AdminDeleteUserRequest CreateAdminDeleteUserRequest(string userPoolId, string username) =>
            new()
            {
                UserPoolId = userPoolId,
                Username = username
            };

        public InitiateSrpAuthRequest CreateInitiateSrpAuthRequest(string password) =>
            new()
            {
                Password = password
            };

        private static string CalcularSecretHash(string clientId, string clientSecret, string nomeUsuario)
        {
            var keyBytes = Encoding.UTF8.GetBytes(clientSecret);
            var messageBytes = Encoding.UTF8.GetBytes(nomeUsuario + clientId);

            using var hmac = new HMACSHA256(keyBytes);
            var hash = hmac.ComputeHash(messageBytes);
            return Convert.ToBase64String(hash);
        }
    }
}
