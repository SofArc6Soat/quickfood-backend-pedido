using Amazon.CognitoIdentityProvider.Model;
using Amazon.Extensions.CognitoAuthentication;

namespace Gateways.Cognito
{
    public interface ICognitoFactory
    {
        SignUpRequest CreateSignUpRequest(string email, string password, string name, string? cpf = null);
        AdminAddUserToGroupRequest CreateAddUserToGroupRequest(string email, string groupName);
        ConfirmSignUpRequest CreateConfirmSignUpRequest(string email, string confirmationCode);
        ForgotPasswordRequest CreateForgotPasswordRequest(string email);
        ConfirmForgotPasswordRequest CreateConfirmForgotPasswordRequest(string email, string confirmationCode, string newPassword);
        ListUsersRequest CreateListUsersRequestByEmail(string userPoolId, string email);
        ListUsersRequest CreateListUsersRequestByAll(string userPoolId, string? paginationToken);
        AdminDeleteUserRequest CreateAdminDeleteUserRequest(string userPoolId, string username);
        InitiateSrpAuthRequest CreateInitiateSrpAuthRequest(string password);
    }
}
