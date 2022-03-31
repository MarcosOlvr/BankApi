namespace Cumbuca.Models
{
    public class Transacao
    {
        public int Id { get; set; }
        public int ContaEnviante { get; set; }
        public int ContaRecebedora { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataDeProcessamento { get; set; } = DateTime.Now;
    }
}
