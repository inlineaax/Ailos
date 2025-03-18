using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Infrastructure.Database.CommandStore.Requests;
using Questao5.Infrastructure.Database.CommandStore.Responses;
using Questao5.Infrastructure.Repositories.Interfaces;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Repositories
{
    public class MovimentoRepository : IMovimentoRepository
    {
        private readonly string _connectionString;
        public MovimentoRepository(DatabaseConfig config)
        {
            _connectionString = config.Name;
        }

        public async Task<SaveMovimentoResponse> SaveMovimentoAsync(SaveMovimentoRequest request)
        {
            using var connection = new SqliteConnection(_connectionString);
            string sql = "INSERT INTO movimento (idmovimento, idcontacorrente, datamovimento, tipomovimento, valor) " +
                         "VALUES (@IdMovimento, @IdContaCorrente, @DataMovimento, @TipoMovimento, @Valor)";
            await connection.ExecuteAsync(sql, request);
            return new SaveMovimentoResponse { Success = true, IdMovimento = request.IdMovimento };
        }
    }
}
