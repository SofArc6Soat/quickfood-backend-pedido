using System.ComponentModel.DataAnnotations;

namespace Gateways.Cognito.Dtos.Request
{
    public class ConfirmarEmailVerificacaoDto
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [EmailAddress(ErrorMessage = "{0} está em um formato inválido.")]
        [StringLength(100, ErrorMessage = "O campo {0} deve conter entre {2} e {1} caracteres.", MinimumLength = 5)]
        [Display(Name = "E-mail")]
        public required string Email { get; set; }

        [Required]
        [Length(6, 6, ErrorMessage = "O campo {0} deve conter {1} caracteres.")]
        public required string CodigoVerificacao { get; set; }
    }
}
