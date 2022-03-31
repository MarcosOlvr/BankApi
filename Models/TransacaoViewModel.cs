namespace Cumbuca.Models
{
    public class TransacaoViewModel
    {
        public int Recebedor { get; set; }
        public decimal Valor { get; set; }
    }

    public class TransacaoByCpf
    {
        public string Recebedor { get; set; }
        public decimal Valor { get; set; }
    }
}
