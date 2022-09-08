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
    [Route("api")]
    public class TransacaoController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        private readonly ITransacaoRepository _transfRepo;

        public TransacaoController(IUserRepository userRepo, ITransacaoRepository transfRepo)
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

            var transacao = _transfRepo.CreateTransacao(model, userId);
            
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

            var transacao = _transfRepo.CreateTransacao(model, userId);
            
            // Retornando 201, pois foi criada uma transferência
            return StatusCode(201, transacao);
        }

        [HttpGet]
        [Route("transacao/{id:int}")]
        public async Task<ActionResult<dynamic>> GetTransacao([FromRoute] int id)
        {
            var transferencia = _transfRepo.GetTransacao(id);
            if (transferencia == null)
                return NotFound();

            return Ok(transferencia);
        }

        [HttpGet]
        [Route("data")]
        public async Task<ActionResult<dynamic>> GetTransacaoByDate([FromQuery]DateTime inicial, [FromQuery]DateTime final)
        {
            var userId = GetIdByClaim();
            if (userId <= 0)
                return BadRequest();

            var transferencias = _transfRepo.GetByDate(inicial, final, userId);

            return Ok(transferencias);
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

            _transfRepo.UpdateTransacao(transferencia.Id);

            return Ok( new { mensagem = "Transação estornada com sucesso!" });
        }

        public int GetIdByClaim()
        {
            var userId = User.Claims.Where(e => e.Type == "UserId").Select(e => e.Value).FirstOrDefault();
            return int.Parse(userId);
        }
    }
}