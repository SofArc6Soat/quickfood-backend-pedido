using Controllers;
using Core.Domain.Notificacoes;
using Core.WebApi.Controller;
using Gateways.Dtos.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize(Policy = "AdminRole")]
    [Route("pedidos")]
    public class PedidosApiController(IPedidoController pedidoController, INotificador notificador) : MainController(notificador)
    {
        [HttpGet]
        public async Task<IActionResult> ObterTodosPedidos(CancellationToken cancellationToken)
        {
            var result = await pedidoController.ObterTodosPedidosAsync(cancellationToken);

            return CustomResponseGet(result);
        }

        [HttpGet("ordenados")]
        public async Task<IActionResult> ObterTodosPedidosOrdenados(CancellationToken cancellationToken)
        {
            var result = await pedidoController.ObterTodosPedidosOrdenadosAsync(cancellationToken);

            return CustomResponseGet(result);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CadastrarPedido(PedidoRequestDto request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return ErrorBadRequestModelState(ModelState);
            }

            var result = await pedidoController.CadastrarPedidoAsync(request, cancellationToken);

            return CustomResponsePost($"pedidos/{request.PedidoId}", request, result);
        }

        [HttpPatch("status/{pedidoId:guid}")]
        public async Task<IActionResult> AlterarStatus([FromRoute] Guid pedidoId, [FromBody] PedidoStatusRequestDto pedidoStatusDto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return ErrorBadRequestModelState(ModelState);
            }

            var result = await pedidoController.AlterarStatusAsync(pedidoId, pedidoStatusDto, cancellationToken);

            return CustomResponsePutPatch(pedidoStatusDto, result);
        }
    }
}
