using System.ComponentModel.DataAnnotations;

namespace Gateways.Dtos.Request
{
    public record PedidoStatusRequestDto
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [AllowedValues("EmPreparacao", "Pronto", "Finalizado", ErrorMessage = "Status inválido.")]
        public required string Status { get; set; }
    }
}