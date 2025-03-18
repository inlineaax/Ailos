using Moq;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Application.Services;
using Questao5.Application.Services.Interfaces;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Database.CommandStore.Requests;
using Questao5.Infrastructure.Database.CommandStore.Responses;
using Questao5.Infrastructure.Database.QueryStore.Requests;
using Questao5.Infrastructure.Database.QueryStore.Responses;
using Questao5.Infrastructure.Repositories.Interfaces;

namespace Questao5.Tests.Services
{
    public class ContaCorrenteServiceTests
    {
        private readonly Mock<IMovimentoRepository> _movimentoRepoMock;
        private readonly Mock<IContaCorrenteQueryRepository> _queryRepoMock;
        private readonly Mock<IContaCorrenteRepository> _contaRepoMock;
        private readonly IContaCorrenteService _service;

        public ContaCorrenteServiceTests()
        {
            _movimentoRepoMock = new Mock<IMovimentoRepository>();
            _queryRepoMock = new Mock<IContaCorrenteQueryRepository>();
            _contaRepoMock = new Mock<IContaCorrenteRepository>();

            _service = new ContaCorrenteService(
                _movimentoRepoMock.Object,
                _queryRepoMock.Object,
                _contaRepoMock.Object);
        }

        [Fact]
        public async Task MovimentarAsync_ValidCommand_ReturnsMovimentoId()
        {
            // Arrange
            var command = new CreateMovimentoCommand
            {
                IdempotencyKey = "test-key",
                NumeroConta = 123,
                TipoMovimento = 'C',
                Valor = 100.0
            };

            var conta = new ContaCorrente
            {
                IdContaCorrente = "valid-account-id",
                Numero = 123,
                Nome = "Conta Teste",
                Ativo = true
            };

            _contaRepoMock.Setup(x => x.GetByNumeroAsync(It.IsAny<int>()))
                          .ReturnsAsync(conta);

            var expectedMovimentoId = "mov-123";
            _movimentoRepoMock.Setup(x => x.SaveMovimentoAsync(It.IsAny<SaveMovimentoRequest>()))
                              .ReturnsAsync(new SaveMovimentoResponse
                              {
                                  Success = true,
                                  IdMovimento = expectedMovimentoId
                              });

            // Act
            CreateMovimentoCommandResponse response = await _service.MovimentarAsync(command);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(expectedMovimentoId, response.IdMovimento);
            _contaRepoMock.Verify(x => x.GetByNumeroAsync(command.NumeroConta), Times.Once);
            _movimentoRepoMock.Verify(x => x.SaveMovimentoAsync(It.IsAny<SaveMovimentoRequest>()), Times.Once);
        }

        [Fact]
        public async Task ConsultarSaldoAsync_ValidQuery_ReturnsCorrectSaldo()
        {
            // Arrange
            var query = new GetSaldoQuery
            {
                NumeroConta = 123
            };

            var conta = new ContaCorrente
            {
                IdContaCorrente = "valid-account-id",
                Numero = 123,
                Nome = "Conta Teste",
                Ativo = true
            };

            _contaRepoMock.Setup(x => x.GetByNumeroAsync(It.IsAny<int>()))
                          .ReturnsAsync(conta);

            var expectedSaldoResponse = new GetSaldoResponse
            {
                NumeroConta = 123,
                NomeTitular = "Conta Teste",
                DataConsulta = DateTime.Now,
                Saldo = 50.0
            };

            _queryRepoMock.Setup(x => x.GetSaldoAsync(It.IsAny<GetSaldoRequest>()))
                          .ReturnsAsync(expectedSaldoResponse);

            // Act
            GetSaldoQueryResponse response = await _service.ConsultarSaldoAsync(query);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(conta.Numero, response.NumeroConta);
            Assert.Equal(conta.Nome, response.NomeTitular);
            Assert.Equal(expectedSaldoResponse.Saldo, response.Saldo);
            _contaRepoMock.Verify(x => x.GetByNumeroAsync(query.NumeroConta), Times.Once);
            _queryRepoMock.Verify(x => x.GetSaldoAsync(It.IsAny<GetSaldoRequest>()), Times.Once);
        }
    }
}
