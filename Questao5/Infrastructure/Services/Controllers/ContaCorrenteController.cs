using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;

namespace Questao5.Infrastructure.Services.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContaCorrenteController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ContaCorrenteController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Realiza a movimentação de uma conta corrente.
        /// </summary>
        /// <param name="command">Dados da movimentação (idempotência, número da conta, tipo e valor).</param>
        /// <returns>ID do movimento gerado.</returns>
        [HttpPost("movimentacao")]
        public async Task<IActionResult> Movimentar([FromBody] CreateMovimentoCommand command)
        {
            try
            {
                CreateMovimentoCommandResponse response = await _mediator.Send(command);
                return Ok(response);
            }
            catch (Exception ex)
            {
                // Em caso de falha, retorna HTTP 400 com mensagem descritiva
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Consulta o saldo de uma conta corrente.
        /// </summary>
        /// <param name="numeroConta">Número da conta corrente.</param>
        /// <returns>Dados da conta e saldo atual.</returns>
        [HttpGet("saldo/{numeroConta}")]
        public async Task<IActionResult> ConsultarSaldo([FromRoute] int numeroConta)
        {
            try
            {
                var query = new GetSaldoQuery { NumeroConta = numeroConta };
                GetSaldoQueryResponse response = await _mediator.Send(query);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
