using FluentValidation.TestHelper;
using Moq;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Validators;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Repositories.Interfaces;

namespace Questao5.Tests.Validators
{
    public class CreateMovimentoCommandValidatorTests
    {
        private readonly CreateMovimentoCommandValidator _validator;
        private readonly Mock<IContaCorrenteRepository> _contaRepoMock;

        public CreateMovimentoCommandValidatorTests()
        {
            _contaRepoMock = new Mock<IContaCorrenteRepository>();
            _validator = new CreateMovimentoCommandValidator(_contaRepoMock.Object);
        }

        [Fact]
        public async Task Should_Have_Error_When_Conta_Not_Found()
        {
            // Arrange
            var command = new CreateMovimentoCommand
            {
                IdempotencyKey = "key",
                NumeroConta = 123,
                TipoMovimento = 'C',
                Valor = 100
            };

            _contaRepoMock.Setup(x => x.GetByNumeroAsync(It.IsAny<int>()))
                          .ReturnsAsync((ContaCorrente)null!);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.NumeroConta)
                  .WithErrorMessage("INVALID_ACCOUNT - Conta não encontrada.");
        }

        [Fact]
        public async Task Should_Have_Error_When_Conta_Inactive()
        {
            // Arrange
            var command = new CreateMovimentoCommand
            {
                IdempotencyKey = "key",
                NumeroConta = 123,
                TipoMovimento = 'C',
                Valor = 100
            };

            _contaRepoMock.Setup(x => x.GetByNumeroAsync(It.IsAny<int>()))
                          .ReturnsAsync(new ContaCorrente { IdContaCorrente = "id", Numero = 123, Nome = "Teste", Ativo = false });

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.NumeroConta)
                  .WithErrorMessage("INACTIVE_ACCOUNT - Conta inativa.");
        }

        [Fact]
        public async Task Should_Have_Error_When_Valor_Not_Positive()
        {
            // Arrange
            var command = new CreateMovimentoCommand
            {
                IdempotencyKey = "key",
                NumeroConta = 123,
                TipoMovimento = 'C',
                Valor = 0
            };

            _contaRepoMock.Setup(x => x.GetByNumeroAsync(It.IsAny<int>()))
                          .ReturnsAsync(new ContaCorrente { IdContaCorrente = "id", Numero = 123, Nome = "Teste", Ativo = true });

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Valor)
                  .WithErrorMessage("INVALID_VALUE - Valor deve ser positivo.");
        }

        [Fact]
        public async Task Should_Have_Error_When_TipoMovimento_Invalid()
        {
            // Arrange
            var command = new CreateMovimentoCommand
            {
                IdempotencyKey = "key",
                NumeroConta = 123,
                TipoMovimento = 'X',
                Valor = 100
            };

            _contaRepoMock.Setup(x => x.GetByNumeroAsync(It.IsAny<int>()))
                          .ReturnsAsync(new ContaCorrente { IdContaCorrente = "id", Numero = 123, Nome = "Teste", Ativo = true });

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.TipoMovimento)
                  .WithErrorMessage("INVALID_TYPE - Tipo de movimento inválido.");
        }

        [Fact]
        public async Task Should_Have_Error_When_IdempotencyKey_Is_Empty()
        {
            // Arrange
            var command = new CreateMovimentoCommand
            {
                IdempotencyKey = "",
                NumeroConta = 123,
                TipoMovimento = 'C',
                Valor = 100
            };

            _contaRepoMock.Setup(x => x.GetByNumeroAsync(It.IsAny<int>()))
                          .ReturnsAsync(new ContaCorrente { IdContaCorrente = "id", Numero = 123, Nome = "Teste", Ativo = true });

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.IdempotencyKey)
                  .WithErrorMessage("A chave de idempotência é obrigatória.");
        }

        [Fact]
        public async Task Should_Pass_For_Valid_Command()
        {
            // Arrange
            var command = new CreateMovimentoCommand
            {
                IdempotencyKey = "key",
                NumeroConta = 123,
                TipoMovimento = 'C',
                Valor = 100
            };

            _contaRepoMock.Setup(x => x.GetByNumeroAsync(It.IsAny<int>()))
                          .ReturnsAsync(new ContaCorrente { IdContaCorrente = "id", Numero = 123, Nome = "Teste", Ativo = true });

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
