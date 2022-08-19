using System.ComponentModel.DataAnnotations;

namespace Desafio.Models.ViewModels
{
    public class LoginViewModel
    {
        [MinLength(11, ErrorMessage = "CPF length is 11")]
        [MaxLength(11, ErrorMessage = "CPF length is 11")]
        public string Cpf { get; set; } 
        public string Senha { get; set; }
    }
}
