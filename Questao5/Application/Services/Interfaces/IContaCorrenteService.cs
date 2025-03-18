using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;

namespace Questao5.Application.Services.Interfaces
{
    public interface IContaCorrenteService
    {
        Task<CreateMovimentoCommandResponse> MovimentarAsync(CreateMovimentoCommand command);
        Task<GetSaldoQueryResponse> ConsultarSaldoAsync(GetSaldoQuery query);
    }
}
