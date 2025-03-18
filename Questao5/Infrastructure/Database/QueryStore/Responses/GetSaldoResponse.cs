namespace Questao5.Infrastructure.Database.QueryStore.Responses
{
    public class GetSaldoResponse
    {
        public int NumeroConta { get; set; }
        public string? NomeTitular { get; set; }
        public DateTime DataConsulta { get; set; }
        public double Saldo { get; set; }
    }
}
