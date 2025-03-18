using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Infrastructure.Database.QueryStore.Requests;
using Questao5.Infrastructure.Database.QueryStore.Responses;
using Questao5.Infrastructure.Repositories.Interfaces;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Repositories
{
    public class ContaCorrenteQueryRepository : IContaCorrenteQueryRepository
    {
        private readonly string _connectionString;
        public ContaCorrenteQueryRepository(DatabaseConfig config)
        {
            _connectionString = config.Name;
        }
        public async Task<GetSaldoResponse> GetSaldoAsync(GetSaldoRequest request)
        {
            using var connection = new SqliteConnection(_connectionString);

            var conta = await connection.QueryFirstOrDefaultAsync<Domain.Entities.ContaCorrente>(
                "SELECT idcontacorrente AS IdContaCorrente, numero, nome, ativo FROM contacorrente WHERE idcontacorrente = @IdConta",
                new { IdConta = request.IdContaCorrente });

            var creditos = await connection.ExecuteScalarAsync<double?>(
                "SELECT SUM(valor) FROM movimento WHERE idcontacorrente = @IdConta AND tipomovimento = 'C'",
                new { IdConta = request.IdContaCorrente });
            var debitos = await connection.ExecuteScalarAsync<double?>(
                "SELECT SUM(valor) FROM movimento WHERE idcontacorrente = @IdConta AND tipomovimento = 'D'",
                new { IdConta = request.IdContaCorrente });
            double saldo = (creditos ?? 0) - (debitos ?? 0);

            return new GetSaldoResponse
            {
                NumeroConta = conta.Numero,
                NomeTitular = conta.Nome,
                DataConsulta = DateTime.Now,
                Saldo = saldo
            };
        }
    }
}
