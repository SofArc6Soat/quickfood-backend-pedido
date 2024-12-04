using Core.Domain.Notificacoes;

namespace Domain.Tests.TestHelpers;

public class NotificadorFake : INotificador
{
    private readonly List<Notificacao> _notificacoes = new();

    public bool TemNotificacao() => _notificacoes.Count > 0;

    public List<Notificacao> ObterNotificacoes() => _notificacoes;

    public void Handle(Notificacao notificacao) => _notificacoes.Add(notificacao);
}