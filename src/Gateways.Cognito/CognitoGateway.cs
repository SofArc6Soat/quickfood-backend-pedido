using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Extensions.CognitoAuthentication;
using Domain.Entities;
using Domain.ValueObjects;
using Gateways.Cognito.Configurations;
using Gateways.Cognito.Dtos.Response;

namespace Gateways.Cognito
{
    public class CognitoGateway : ICognitoGateway
    {
        private readonly IAmazonCognitoIdentityProvider _cognitoClientIdentityProvider;
        private readonly ICognitoFactory _cognitoFactory;

        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _userPoolId;

        public CognitoGateway(IAmazonCognitoIdentityProvider cognitoClientIdentityProvider, ICognitoFactory cognitoFactory, ICognitoConfig cognitoSettings)
        {
            _cognitoClientIdentityProvider = cognitoClientIdentityProvider;
            _cognitoFactory = cognitoFactory;

            var _cognitoSettings = cognitoSettings;

            _clientId = _cognitoSettings.ClientId;
            _clientSecret = _cognitoSettings.ClientSecret;
            _userPoolId = _cognitoSettings.UserPoolId;
        }

        public async Task<bool> CriarUsuarioClienteAsync(Cliente cliente, string senha, CancellationToken cancellationToken)
        {
            if (await VerificarSeCpfExisteAsync(cliente.Cpf, cancellationToken) || await VerificarSeEmailExisteAsync(cliente.Email, cancellationToken))
            {
                return false;
            }

            var signUpRequest = _cognitoFactory.CreateSignUpRequest(cliente.Email, senha, cliente.Nome, cliente.Cpf);
            var adminAddUserToGroupRequest = _cognitoFactory.CreateAddUserToGroupRequest(cliente.Email, "cliente");

            return await CriarUsuarioCognitoAsync(signUpRequest, adminAddUserToGroupRequest, cliente.Email, cancellationToken);
        }

        public async Task<bool> CriarUsuarioFuncionarioAsync(Funcionario funcionario, string senha, CancellationToken cancellationToken)
        {
            if (await VerificarSeEmailExisteAsync(funcionario.Email, cancellationToken))
            {
                return false;
            }

            var signUpRequest = _cognitoFactory.CreateSignUpRequest(funcionario.Email, senha, funcionario.Nome);
            var adminAddUserToGroupRequest = _cognitoFactory.CreateAddUserToGroupRequest(funcionario.Email, "admin");

            return await CriarUsuarioCognitoAsync(signUpRequest, adminAddUserToGroupRequest, funcionario.Email, cancellationToken);
        }

        public async Task<bool> ConfirmarEmailVerificacaoAsync(EmailVerificacao emailVerificacao, CancellationToken cancellationToken)
        {
            var confirmSignUpRequest = _cognitoFactory.CreateConfirmSignUpRequest(emailVerificacao.Email, emailVerificacao.CodigoVerificacao);

            try
            {
                var response = await _cognitoClientIdentityProvider.ConfirmSignUpAsync(confirmSignUpRequest, cancellationToken);

                return response is not null && response.HttpStatusCode == System.Net.HttpStatusCode.OK;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> SolicitarRecuperacaoSenhaAsync(RecuperacaoSenha recuperacaoSenha, CancellationToken cancellationToken)
        {
            var forgotPasswordRequest = _cognitoFactory.CreateForgotPasswordRequest(recuperacaoSenha.Email);

            try
            {
                var response = await _cognitoClientIdentityProvider.ForgotPasswordAsync(forgotPasswordRequest, cancellationToken);

                return response is not null && response.HttpStatusCode == System.Net.HttpStatusCode.OK;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> EfetuarResetSenhaAsync(ResetSenha resetSenha, CancellationToken cancellationToken)
        {
            var confirmForgotPasswordRequest = _cognitoFactory.CreateConfirmForgotPasswordRequest(resetSenha.Email, resetSenha.CodigoVerificacao, resetSenha.NovaSenha);

            try
            {
                var response = await _cognitoClientIdentityProvider.ConfirmForgotPasswordAsync(confirmForgotPasswordRequest, cancellationToken);

                return response is not null && response.HttpStatusCode == System.Net.HttpStatusCode.OK;
            }
            catch
            {
                return false;
            }
        }

        public async Task<TokenUsuario?> IdentifiqueSeAsync(string? email, string? cpf, string senha, CancellationToken cancellationToken)
        {
            var userPool = new CognitoUserPool(_userPoolId, _clientId, _cognitoClientIdentityProvider);

            var userId = string.Empty;

            if (email is not null || cpf is not null)
            {
                if (email is not null)
                {
                    userId = await ObertUsuarioCognitoPorEmailAsync(email, cancellationToken);
                }

                if (cpf is not null)
                {
                    userId = await ObterUserIdPorCpfAsync(cpf, cancellationToken);

                }

                if (!string.IsNullOrEmpty(userId))
                {
                    var cognitoUser = new CognitoUser(userId, _clientId, userPool, _cognitoClientIdentityProvider, _clientSecret);

                    var authRequest = _cognitoFactory.CreateInitiateSrpAuthRequest(senha);

                    try
                    {
                        var respose = await cognitoUser.StartWithSrpAuthAsync(authRequest, cancellationToken);

                        if (respose is null || respose.ChallengeName == ChallengeNameType.NEW_PASSWORD_REQUIRED)
                        {
                            return null;
                        }

                        var timeSpan = TimeSpan.FromSeconds(respose.AuthenticationResult.ExpiresIn);
                        var expiry = DateTimeOffset.UtcNow + timeSpan;

                        return new()
                        {
                            Email = email,
                            Cpf = cpf,
                            AccessToken = respose.AuthenticationResult.AccessToken,
                            RefreshToken = respose.AuthenticationResult.RefreshToken,
                            Expiry = expiry
                        };
                    }
                    catch (NotAuthorizedException)
                    {
                        throw new NotAuthorizedException("Credenciais inválidas.");
                    }
                    catch (UserNotConfirmedException)
                    {
                        throw new UserNotConfirmedException("Usuário não confirmado. Por favor, verifique seu e-mail.");
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }

                return null;
            }

            return null;
        }

        public async Task<bool> DeletarUsuarioCognitoAsync(string email, CancellationToken cancellationToken)
        {
            try
            {
                var username = await ObertUsuarioCognitoPorEmailAsync(email, cancellationToken);

                if (username == null)
                {
                    return false;
                }

                var adminDeleteUserRequest = _cognitoFactory.CreateAdminDeleteUserRequest(_userPoolId, username);

                await _cognitoClientIdentityProvider.AdminDeleteUserAsync(adminDeleteUserRequest, cancellationToken);

                return true;
            }
            catch (UserNotFoundException)
            {
                return false;
            }
            catch
            {
                return false;
            }
        }

        private async Task<bool> CriarUsuarioCognitoAsync(SignUpRequest signUpRequest, AdminAddUserToGroupRequest addToGroupRequest, string email, CancellationToken cancellationToken)
        {
            try
            {
                var signUpResponse = await _cognitoClientIdentityProvider.SignUpAsync(signUpRequest, cancellationToken);

                if (signUpResponse.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    var addToGroupResponse = await _cognitoClientIdentityProvider.AdminAddUserToGroupAsync(addToGroupRequest, cancellationToken);

                    if (addToGroupResponse.HttpStatusCode == System.Net.HttpStatusCode.OK)
                    {
                        return true;
                    }

                    await DeletarUsuarioCognitoAsync(email, cancellationToken);
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        private async Task<bool> VerificarSeEmailExisteAsync(string email, CancellationToken cancellationToken)
        {
            try
            {
                var request = _cognitoFactory.CreateListUsersRequestByEmail(_userPoolId, email);

                var response = await _cognitoClientIdentityProvider.ListUsersAsync(request, cancellationToken);

                return response.Users.Count != 0;
            }
            catch (Exception ex)
            {
                throw new HttpRequestException(ex.Message);
            }
        }

        private async Task<bool> VerificarSeCpfExisteAsync(string cpf, CancellationToken cancellationToken)
        {
            var usuarios = await ObterTodosUsuariosCognitoAsync(cancellationToken);

            var usuariosComCpf = usuarios.FirstOrDefault(usuario =>
                usuario.Attributes.Any(attribute =>
                    attribute.Name == "custom:cpf" && attribute.Value == cpf));

            return usuariosComCpf != null;
        }

        private async Task<string> ObterUserIdPorCpfAsync(string cpf, CancellationToken cancellationToken)
        {
            var usuarios = await ObterTodosUsuariosCognitoAsync(cancellationToken);

            var usuariosComCpf = usuarios.FirstOrDefault(usuario =>
                    usuario.Attributes.Any(attribute =>
                        attribute.Name == "custom:cpf" && attribute.Value == cpf));

            if (usuariosComCpf == null)
            {
                return string.Empty;
            }

            var emailAttribute = usuariosComCpf.Attributes.FirstOrDefault(attr => attr.Name == "email");
            return emailAttribute?.Value;
        }

        private async Task<List<UserType>> ObterTodosUsuariosCognitoAsync(CancellationToken cancellationToken)
        {
            try
            {
                var usuarios = new List<UserType>();
                string? paginationToken = null;

                do
                {
                    var request = _cognitoFactory.CreateListUsersRequestByAll(_userPoolId, paginationToken);

                    var response = await _cognitoClientIdentityProvider.ListUsersAsync(request, cancellationToken);

                    usuarios.AddRange(response.Users);

                    paginationToken = response.PaginationToken;
                }
                while (!string.IsNullOrEmpty(paginationToken));

                return usuarios;
            }
            catch (Exception ex)
            {
                throw new HttpRequestException(ex.Message);
            }
        }

        private async Task<string?> ObertUsuarioCognitoPorEmailAsync(string email, CancellationToken cancellationToken)
        {
            var request = _cognitoFactory.CreateListUsersRequestByEmail(_userPoolId, email);

            var response = await _cognitoClientIdentityProvider.ListUsersAsync(request, cancellationToken);

            var usuario = response.Users.FirstOrDefault();
            return usuario?.Username;
        }
    }
}