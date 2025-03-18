using FluentValidation;
using Questao5.Application.Commands.Requests;
using Questao5.Infrastructure.Repositories.Interfaces;

namespace Questao5.Application.Validators
{
    public class CreateMovimentoCommandValidator : AbstractValidator<CreateMovimentoCommand>
    {
        public CreateMovimentoCommandValidator(IContaCorrenteRepository contaCorrenteRepository)
        {
            RuleFor(x => x.NumeroConta)
                .MustAsync(async (numero, cancellation) =>
                {
                    var conta = await contaCorrenteRepository.GetByNumeroAsync(numero);
                    return conta != null;
                })
                .WithMessage("INVALID_ACCOUNT - Conta não encontrada.");
            RuleFor(x => x.NumeroConta)
                .MustAsync(async (numero, cancellation) =>
                {
                    var conta = await contaCorrenteRepository.GetByNumeroAsync(numero);
                    return conta != null && conta.Ativo;
                })
                .WithMessage("INACTIVE_ACCOUNT - Conta inativa.");
            RuleFor(x => x.Valor)
                .GreaterThan(0)
                .WithMessage("INVALID_VALUE - Valor deve ser positivo.");
            RuleFor(x => x.TipoMovimento)
                .Must(tipo => tipo == 'C' || tipo == 'D')
                .WithMessage("INVALID_TYPE - Tipo de movimento inválido.");
            RuleFor(x => x.IdempotencyKey)
                .NotEmpty()
                .WithMessage("A chave de idempotência é obrigatória.");
        }
    }
}
