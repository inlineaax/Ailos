using Questao5.Infrastructure.Database.QueryStore.Requests;
using Questao5.Infrastructure.Database.QueryStore.Responses;

namespace Questao5.Infrastructure.Repositories.Interfaces
{
    public interface IContaCorrenteQueryRepository
    {
        Task<GetSaldoResponse> GetSaldoAsync(GetSaldoRequest request);
    }
}
