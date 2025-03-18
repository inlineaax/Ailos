using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Application.Services.Interfaces;

namespace Questao5.Application.Handlers
{
    public class CreateMovimentoCommandHandler : IRequestHandler<CreateMovimentoCommand, CreateMovimentoCommandResponse>
    {
        private readonly IContaCorrenteService _contaService;
        private readonly IValidator<CreateMovimentoCommand> _validator;

        public CreateMovimentoCommandHandler(IContaCorrenteService contaService, IValidator<CreateMovimentoCommand> validator)
        {
            _contaService = contaService;
            _validator = validator;
        }

        public async Task<CreateMovimentoCommandResponse> Handle(CreateMovimentoCommand request, CancellationToken cancellationToken)
        {
            ValidationResult validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            return await _contaService.MovimentarAsync(request);
        }
    }
}
