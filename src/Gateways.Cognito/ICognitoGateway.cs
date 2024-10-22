using Domain.Entities;
using Domain.ValueObjects;
using Gateways.Cognito.Dtos.Response;

namespace Gateways.Cognito
{
    public interface ICognitoGateway
    {
        Task<bool> CriarUsuarioClienteAsync(Cliente cliente, string senha, CancellationToken cancellationToken);
        Task<bool> CriarUsuarioFuncionarioAsync(Funcionario funcionario, string senha, CancellationToken cancellationToken);
        Task<TokenUsuario?> IdentifiqueSeAsync(string? email, string? cpf, string senha, CancellationToken cancellationToken);
        Task<bool> ConfirmarEmailVerificacaoAsync(EmailVerificacao emailVerificacao, CancellationToken cancellationToken);
        Task<bool> SolicitarRecuperacaoSenhaAsync(RecuperacaoSenha recuperacaoSenha, CancellationToken cancellationToken);
        Task<bool> EfetuarResetSenhaAsync(ResetSenha resetSenha, CancellationToken cancellationToken);
    }
}
