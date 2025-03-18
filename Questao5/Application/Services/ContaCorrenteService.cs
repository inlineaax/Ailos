using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Application.Services.Interfaces;
using Questao5.Infrastructure.Database.CommandStore.Requests;
using Questao5.Infrastructure.Repositories.Interfaces;

namespace Questao5.Application.Services
{
    public class ContaCorrenteService : IContaCorrenteService
    {
        private readonly IMovimentoRepository _movimentoRepository;
        private readonly IContaCorrenteQueryRepository _queryRepository;
        private readonly IContaCorrenteRepository _contaRepository;

        public ContaCorrenteService(
            IMovimentoRepository movimentoRepository,
            IContaCorrenteQueryRepository queryRepository,
            IContaCorrenteRepository contaRepository)
        {
            _movimentoRepository = movimentoRepository;
            _queryRepository = queryRepository;
            _contaRepository = contaRepository;
        }

        public async Task<CreateMovimentoCommandResponse> MovimentarAsync(CreateMovimentoCommand command)
        {
            var conta = await _contaRepository.GetByNumeroAsync(command.NumeroConta);

            var idMovimento = Guid.NewGuid().ToString();
            var dataMovimento = DateTime.Now.ToString("dd/MM/yyyy");
            var saveRequest = new SaveMovimentoRequest
            {
                IdMovimento = idMovimento,
                IdContaCorrente = conta.IdContaCorrente,
                DataMovimento = dataMovimento,
                TipoMovimento = command.TipoMovimento.ToString(),
                Valor = command.Valor
            };

            var result = await _movimentoRepository.SaveMovimentoAsync(saveRequest);

            return new CreateMovimentoCommandResponse { IdMovimento = result.IdMovimento };
        }

        public async Task<GetSaldoQueryResponse> ConsultarSaldoAsync(GetSaldoQuery query)
        {
            var conta = await _contaRepository.GetByNumeroAsync(query.NumeroConta);

            var getSaldoRequest = new Infrastructure.Database.QueryStore.Requests.GetSaldoRequest
            {
                IdContaCorrente = conta.IdContaCorrente
            };

            var saldoResponse = await _queryRepository.GetSaldoAsync(getSaldoRequest);

            return new GetSaldoQueryResponse
            {
                NumeroConta = conta.Numero,
                NomeTitular = conta.Nome,
                DataConsulta = saldoResponse.DataConsulta,
                Saldo = saldoResponse.Saldo
            };
        }
    }
}
