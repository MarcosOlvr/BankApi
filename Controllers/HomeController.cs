using Cumbuca.Data;
using Cumbuca.Models;
using Cumbuca.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Cumbuca.Controllers
{
    [ApiController]
    [Route("v1")]
    public class HomeController : ControllerBase
    {
        private readonly AppDbContext _db;

        public HomeController(AppDbContext db)
        {
            _db = db;
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<dynamic>> AuthenticateAsync([FromBody]LoginViewModel model)
        {
            var user = UserLogin(model.Cpf, model.Senha);

            if (user == null)
                return NotFound(new { mensagem = "Usuário ou senha inválidos" });

            var token = TokenService.GenerateToken(user);

            // Oculta a senha na hora de retornar
            user.Senha = "";

            return new
            {
                user = user,
                token = token
            };
        }

        [HttpPost]
        [Route("cadastrar")]
        public async Task<ActionResult<dynamic>> CreateUserAsync([FromBody]User model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var userFind = _db.User.FirstOrDefault(x => x.Cpf == model.Cpf);
            if (userFind != null)
                return BadRequest(new { mensagem = "Este CPF já esta sendo usado!" });

            CreateUser(model);
            
            return StatusCode(201, new { mensagem = "Usuário criado!" });
        }

        [HttpGet]
        [Authorize]
        [Route("saldo")]
        public async Task<ActionResult<dynamic>> VerSaldo()
        {
            var userId = GetIdByClaim();
            if (userId <= 0)
                return BadRequest();

            var user = GetUserById(userId);
            if (user == null)
                return NotFound();

            var saldo = user.SaldoInicial;
            return Ok(new { saldo = saldo });
        }

        [HttpPost]
        [Authorize]
        [Route("transacao")]
        public async Task<ActionResult<dynamic>> TransacaoByIdAsync([FromBody]TransacaoViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var userId = GetIdByClaim();

            if (userId <= 0)
                return BadRequest();

            var enviante = GetUserById(userId);
            var recebedor = GetUserById(model.Recebedor);
            if (enviante == null || recebedor == null)
                return NotFound(new { mensagem = "Usuário não encontrado!" });

            var saldo = enviante.SaldoInicial;
            if (saldo <= 0 || saldo < model.Valor)
                return BadRequest(new { mensagem = "Saldo insuficiente!" });

            enviante.SaldoInicial -= model.Valor;
            recebedor.SaldoInicial += model.Valor;

            var tuple = new Tuple<User, User>(recebedor, enviante);
            var transacao = CreateTransacao(tuple, model.Valor);
            SalvarTransacao(transacao);
            
            // Retornando 201, pois foi criada uma transferência
            return StatusCode(201, new { mensagem = "Transação completa!" });
        }
        
        [HttpPost]
        [Authorize]
        [Route("transacao/cpf")]
        public async Task<ActionResult<dynamic>> TransacaoByCpfAsync([FromBody]TransacaoByCpf model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var userId = GetIdByClaim();

            if (userId <= 0)
                return BadRequest();

            var enviante = GetUserById(userId);
            var recebedor = GetUserByCpf(model.Recebedor);
            if (enviante == null || recebedor == null)
                return NotFound(new { mensagem = "Usuário não encontrado!" });

            var saldo = enviante.SaldoInicial;
            if (saldo <= 0 || saldo < model.Valor)
                return BadRequest(new { mensagem = "Saldo insuficiente!" });

            enviante.SaldoInicial -= model.Valor;
            recebedor.SaldoInicial += model.Valor;

            var tuple = new Tuple<User, User>(recebedor, enviante);
            var transacao = CreateTransacao(tuple, model.Valor);
            SalvarTransacao(transacao);
            
            // Retornando 201, pois foi criada uma transferência
            return StatusCode(201, new { mensagem = "Transação completa!" });
        }

        [HttpGet]
        [Authorize]
        [Route("data")]
        public async Task<ActionResult<dynamic>> GetTransacaoByDate([FromQuery]DateTime inicial, [FromQuery]DateTime final)
        {
            var userId = GetIdByClaim();
            if (userId <= 0)
                return BadRequest();

            var transacoes = _db.Transacoes.Where(x => x.ContaEnviante == userId || x.ContaRecebedora == userId).ToList();

            var minhasTransferencias = new List<Transacao>();
            foreach (var obj in transacoes)
            {
                if (obj.DataDeProcessamento >= inicial && obj.DataDeProcessamento <= final)
                {
                    minhasTransferencias.Add(obj);
                }
            }

            return Ok(minhasTransferencias);
        }

        public User UserLogin(string cpf, string senha)
        {
            var user = _db.User.FirstOrDefault(x => x.Cpf == cpf && x.Senha == senha);

            if (user == null)
                return null;

            return user;
        }
        
        public User GetUserById(int id)
        {
            var user = _db.User.Find(id);
            return user;
        }
        
        public User GetUserByCpf(string cpf)
        {
            var user = _db.User.FirstOrDefault(x => x.Cpf == cpf);
            return user;
        }
        
        public int GetIdByClaim()
        {
            var userId = User.Claims.Where(e => e.Type == "UserId").Select(e => e.Value).FirstOrDefault();
            return int.Parse(userId);
        }

        public void CreateUser(User user)
        {
            if (user == null)
                return;

            _db.User.Add(user);
            _db.SaveChanges();
        }

        public Transacao CreateTransacao(Tuple<User, User> usuarios, decimal valor)
        {
            // O item 1 dessa tupla deve ser sempre o RECEBEDOR
            // O item 2 dessa tupla deve ser sempre o ENVIANTE
            Transacao transacao = new Transacao()
            {
                ContaRecebedora = usuarios.Item1.Id,
                ContaEnviante = usuarios.Item2.Id,
               Valor = valor
            };

            return transacao;
        }

        public void SalvarTransacao(Transacao model)
        {
            if (model == null)
                return;

            _db.Transacoes.Add(model);
            _db.SaveChanges();
        }
    }
}