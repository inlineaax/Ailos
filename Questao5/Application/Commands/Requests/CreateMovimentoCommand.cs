using MediatR;
using Questao5.Application.Commands.Responses;

namespace Questao5.Application.Commands.Requests
{
    public class CreateMovimentoCommand : IRequest<CreateMovimentoCommandResponse>
    {
        public string? IdempotencyKey { get; set; }
        public int NumeroConta { get; set; }
        public char TipoMovimento { get; set; }
        public double Valor { get; set; }
    }
}
