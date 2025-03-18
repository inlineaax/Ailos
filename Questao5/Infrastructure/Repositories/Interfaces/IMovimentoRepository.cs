using Questao5.Infrastructure.Database.CommandStore.Requests;
using Questao5.Infrastructure.Database.CommandStore.Responses;

namespace Questao5.Infrastructure.Repositories.Interfaces
{
    public interface IMovimentoRepository
    {
        Task<SaveMovimentoResponse> SaveMovimentoAsync(SaveMovimentoRequest request);
    }
}
