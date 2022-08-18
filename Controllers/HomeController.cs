using BankApi.Repositories.Contracts;
using Desafio.Models;
using Desafio.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Desafio.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        private readonly ITransacaoRepository _transfRepo;

        public HomeController(IUserRepository userRepo, ITransacaoRepository transfRepo)
        {
            _userRepo = userRepo;
            _transfRepo = transfRepo;
        }

        [HttpGet]
        [Route("saldo")]
        public async Task<ActionResult<dynamic>> VerSaldo()
        {
            var userId = GetIdByClaim();
            if (userId <= 0)
                return BadRequest();

            var user = _userRepo.GetUser(userId);
            if (user == null)
                return NotFound();

            var saldo = user.SaldoInicial;
            return Ok(new { saldo = saldo });
        }

        [HttpPost]
        [Route("transacao")]
        public async Task<ActionResult<dynamic>> TransacaoByIdAsync([FromBody]TransacaoViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var userId = GetIdByClaim();

            if (userId <= 0)
                return BadRequest();

            var enviante = _userRepo.GetUser(userId);
            var recebedor = _userRepo.GetUser(model.Recebedor);
            if (enviante == null || recebedor == null)
                return NotFound(new { mensagem = "Usuário não encontrado!" });

            var saldo = enviante.SaldoInicial;
            if (saldo <= 0 || saldo < model.Valor)
                return BadRequest(new { mensagem = "Saldo insuficiente!" });

            enviante.SaldoInicial -= model.Valor;
            recebedor.SaldoInicial += model.Valor;

            var tuple = new Tuple<User, User>(recebedor, enviante);
            var transacao = _transfRepo.CreateTransacao(tuple, model.Valor);
            _transfRepo.SalvarTransacao(transacao);
            
            // Retornando 201, pois foi criada uma transferência
            return StatusCode(201, transacao);
        }
        
        [HttpPost]
        [Route("transacao/cpf")]
        public async Task<ActionResult<dynamic>> TransacaoByCpfAsync([FromBody]TransacaoByCpf model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var userId = GetIdByClaim();

            if (userId <= 0)
                return BadRequest();

            var enviante = _userRepo.GetUser(userId);
            var recebedor = _userRepo.GetUser(model.Recebedor);
            if (enviante == null || recebedor == null)
                return NotFound(new { mensagem = "Usuário não encontrado!" });

            var saldo = enviante.SaldoInicial;
            if (saldo <= 0 || saldo < model.Valor)
                return BadRequest(new { mensagem = "Saldo insuficiente!" });

            enviante.SaldoInicial -= model.Valor;
            recebedor.SaldoInicial += model.Valor;

            var tuple = new Tuple<User, User>(recebedor, enviante);
            var transacao = _transfRepo.CreateTransacao(tuple, model.Valor);
            _transfRepo.SalvarTransacao(transacao);
            
            // Retornando 201, pois foi criada uma transferência
            return StatusCode(201, transacao);
        }

        [HttpGet]
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

        [HttpPatch]
        [Route("estorno/{id}")]
        public async Task<ActionResult<dynamic>> Estorno(int id)
        {
            var transferencia = _transfRepo.GetTransacao(id);
            if (transferencia == null)
                return NotFound();

            if (transferencia.PodeSerEstornada == false)
                return BadRequest( new { mensagem = "Esta transação já foi estornada!" });

            var userId = GetIdByClaim();
            if (transferencia.ContaEnviante != userId)
                return BadRequest();
            
            var user = _userRepo.GetUser(userId);
            var recebedor = _userRepo.GetUser(transferencia.ContaRecebedora);

            if (user == null || recebedor == null)
                return NotFound();

            _transfRepo.UpdateTransacao(transferencia);

            return Ok( new { mensagem = "Transação estornada com sucesso!" });
        }

        public int GetIdByClaim()
        {
            var userId = User.Claims.Where(e => e.Type == "UserId").Select(e => e.Value).FirstOrDefault();
            return int.Parse(userId);
        }
    }
}