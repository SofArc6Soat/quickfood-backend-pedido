using Core.Domain.Notificacoes;

namespace Domain.Tests.Base;

public class NotificadorTests
{
    [Fact]
    public void Handle_ShouldAddNotificacao()
    {
        // Arrange
        var notificador = new Notificador();
        var notificacao = new Notificacao("Teste de notificação");

        // Act
        notificador.Handle(notificacao);

        // Assert
        Assert.Single(notificador.ObterNotificacoes());
        Assert.Equal("Teste de notificação", notificador.ObterNotificacoes()[0].Mensagem);
    }

    [Fact]
    public void ObterNotificacoes_ShouldReturnAllNotificacoes()
    {
        // Arrange
        var notificador = new Notificador();
        var notificacao1 = new Notificacao("Notificação 1");
        var notificacao2 = new Notificacao("Notificação 2");

        // Act
        notificador.Handle(notificacao1);
        notificador.Handle(notificacao2);
        var notificacoes = notificador.ObterNotificacoes();

        // Assert
        Assert.Equal(2, notificacoes.Count);
        Assert.Equal("Notificação 1", notificacoes[0].Mensagem);
        Assert.Equal("Notificação 2", notificacoes[1].Mensagem);
    }

    [Fact]
    public void TemNotificacao_ShouldReturnTrue_WhenThereAreNotificacoes()
    {
        // Arrange
        var notificador = new Notificador();
        var notificacao = new Notificacao("Teste de notificação");

        // Act
        notificador.Handle(notificacao);

        // Assert
        Assert.True(notificador.TemNotificacao());
    }

    [Fact]
    public void TemNotificacao_ShouldReturnFalse_WhenThereAreNoNotificacoes()
    {
        // Arrange
        var notificador = new Notificador();

        // Act
        var result = notificador.TemNotificacao();

        // Assert
        Assert.False(result);
    }
}