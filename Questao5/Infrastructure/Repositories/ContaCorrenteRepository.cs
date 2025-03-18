using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Repositories.Interfaces;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Repositories
{
    public class ContaCorrenteRepository : IContaCorrenteRepository
    {
        private readonly string _connectionString;

        public ContaCorrenteRepository(DatabaseConfig config)
        {
            _connectionString = config.Name;
        }

        public async Task<ContaCorrente> GetByNumeroAsync(int numero)
        {
            using var connection = new SqliteConnection(_connectionString);
            string sql = @"SELECT idcontacorrente AS IdContaCorrente, numero, nome, ativo 
                           FROM contacorrente 
                           WHERE numero = @Numero";
            return await connection.QueryFirstOrDefaultAsync<ContaCorrente>(sql, new { Numero = numero });
        }
    }
}
