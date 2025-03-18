using MediatR;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Application.Services.Interfaces;

namespace Questao5.Application.Handlers
{
    public class GetSaldoQueryHandler : IRequestHandler<GetSaldoQuery, GetSaldoQueryResponse>
    {
        private readonly IContaCorrenteService _contaService;

        public GetSaldoQueryHandler(IContaCorrenteService contaService)
        {
            _contaService = contaService;
        }

        public async Task<GetSaldoQueryResponse> Handle(GetSaldoQuery request, CancellationToken cancellationToken)
        {
            return await _contaService.ConsultarSaldoAsync(request);
        }
    }
}
