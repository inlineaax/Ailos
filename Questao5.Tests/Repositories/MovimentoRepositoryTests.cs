using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Infrastructure.Repositories;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Tests.Repositories
{
    public class MovimentoRepositoryTests
    {
        private readonly string _connectionString = "Data Source=InMemorySample;Mode=Memory;Cache=Shared";

        private async Task SetupDatabaseAsync(SqliteConnection connection)
        {
            await connection.ExecuteAsync(
                "CREATE TABLE IF NOT EXISTS movimento (" +
                "idmovimento TEXT PRIMARY KEY, " +
                "idcontacorrente TEXT NOT NULL, " +
                "datamovimento TEXT NOT NULL, " +
                "tipomovimento TEXT NOT NULL, " +
                "valor REAL NOT NULL" +
                ");");
        }

        [Fact]
        public async Task SaveMovimentoAsync_InsertsDataAndReturnsResponse()
        {
            // Arrange
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();
            await SetupDatabaseAsync(connection);

            var config = new DatabaseConfig { Name = _connectionString };

            var repository = new MovimentoRepository(config);

            var request = new Questao5.Infrastructure.Database.CommandStore.Requests.SaveMovimentoRequest
            {
                IdMovimento = "mov-001",
                IdContaCorrente = "conta-001",
                DataMovimento = "01/01/2023",
                TipoMovimento = "C",
                Valor = 100.0
            };

            // Act
            var response = await repository.SaveMovimentoAsync(request);

            // Assert
            Assert.True(response.Success);
            Assert.Equal("mov-001", response.IdMovimento);

            var inserted = await connection.QueryFirstOrDefaultAsync<string>(
                "SELECT idmovimento FROM movimento WHERE idmovimento = @Id",
                new { Id = "mov-001" });
            Assert.Equal("mov-001", inserted);
        }
    }
}
