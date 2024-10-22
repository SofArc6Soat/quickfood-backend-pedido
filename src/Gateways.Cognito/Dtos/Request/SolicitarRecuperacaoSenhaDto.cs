using System.ComponentModel.DataAnnotations;

namespace Gateways.Cognito.Dtos.Request
{
    public record SolicitarRecuperacaoSenhaDto
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [EmailAddress(ErrorMessage = "{0} está em um formato inválido.")]
        [StringLength(100, ErrorMessage = "O campo {0} deve conter entre {2} e {1} caracteres.", MinimumLength = 5)]
        [Display(Name = "E-mail")]
        public required string Email { get; set; }
    }
}
