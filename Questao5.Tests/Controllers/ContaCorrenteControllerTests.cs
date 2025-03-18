using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Infrastructure.Services.Controllers;

namespace Questao5.Tests.Controllers
{
    public class ContaCorrenteControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ContaCorrenteController _controller;

        public ContaCorrenteControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new ContaCorrenteController(_mediatorMock.Object);
        }

        [Fact]
        public async Task Movimentar_ReturnsOk_WithResponse()
        {
            // Arrange
            var command = new CreateMovimentoCommand
            {
                IdempotencyKey = "key",
                NumeroConta = 123,
                TipoMovimento = 'C',
                Valor = 100
            };

            var response = new CreateMovimentoCommandResponse { IdMovimento = "mov-123" };

            _mediatorMock.Setup(m => m.Send(It.IsAny<CreateMovimentoCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(response);

            // Act
            var result = await _controller.Movimentar(command) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(response, result.Value);
        }

        [Fact]
        public async Task ConsultarSaldo_ReturnsOk_WithResponse()
        {
            // Arrange
            int numeroConta = 123;
            var response = new GetSaldoQueryResponse
            {
                NumeroConta = 123,
                NomeTitular = "Conta Teste",
                DataConsulta = DateTime.Now,
                Saldo = 50.0
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetSaldoQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(response);

            // Act
            var result = await _controller.ConsultarSaldo(numeroConta) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(response, result.Value);
        }

        [Fact]
        public async Task Movimentar_ReturnsBadRequest_OnException()
        {
            // Arrange
            var command = new CreateMovimentoCommand
            {
                IdempotencyKey = "key",
                NumeroConta = 123,
                TipoMovimento = 'C',
                Valor = 100
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<CreateMovimentoCommand>(), It.IsAny<CancellationToken>()))
                         .ThrowsAsync(new Exception("Test error"));

            // Act
            var result = await _controller.Movimentar(command) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Contains("Test error", result.Value!.ToString());
        }

        [Fact]
        public async Task ConsultarSaldo_ReturnsBadRequest_OnException()
        {
            // Arrange
            int numeroConta = 123;
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetSaldoQuery>(), It.IsAny<CancellationToken>()))
                         .ThrowsAsync(new Exception("Test error"));

            // Act
            var result = await _controller.ConsultarSaldo(numeroConta) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Contains("Test error", result.Value!.ToString());
        }
    }
}
