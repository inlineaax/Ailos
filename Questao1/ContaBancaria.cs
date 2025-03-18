using System.Globalization;

namespace Questao1
{
    class ContaBancaria
    {
        public int Numero { get; private set; }
        public string Titular { get; set; }
        public double Saldo { get; private set; }

        // sem depósito inicial
        public ContaBancaria(int numero, string titular)
        {
            Numero = numero;
            Titular = titular;
            Saldo = 0.0;
        }

        // com depósito inicial.
        public ContaBancaria(int numero, string titular, double depositoInicial) : this(numero, titular)
        {
            Deposito(depositoInicial);
        }

        // Método para realizar depósitos.
        public void Deposito(double quantia)
        {
            Saldo += quantia;
        }

        // Método para realizar saques.
        public void Saque(double quantia)
        {
            Saldo -= quantia + 3.50;
        }

        // exibir os dados da conta.
        public override string ToString()
        {
            return "Conta " + Numero
                + ", Titular: " + Titular
                + ", Saldo: $ " + Saldo.ToString("F2", CultureInfo.InvariantCulture);
        }
    }
}
