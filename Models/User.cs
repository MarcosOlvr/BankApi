using System.ComponentModel.DataAnnotations;

namespace Desafio.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }

        [MinLength(11, ErrorMessage = "CPF length is 11")]
        [MaxLength(11, ErrorMessage = "CPF length is 11")]
        public string Cpf { get; set; }
        public string Senha { get; set; }
        public decimal SaldoInicial { get; set; }
        public DateTime CriadoEm { get; set; } = DateTime.Now;
    }
}
