using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankApi.Repositories.Contracts;
using Desafio.Models;
using Desafio.Models.ViewModels;
using Desafio.Services;
using Microsoft.AspNetCore.Mvc;

namespace BankApi.Controllers
{
    [ApiController]
    [Route("api")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepo;

        public UserController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<dynamic>> AuthenticateAsync([FromBody]LoginViewModel model)
        {
            var user = _userRepo.UserLogin(model.Cpf, model.Senha);

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
                return NotFound();

            var userFind = _userRepo.GetUser(model.Cpf);
            if (userFind != null)
                return BadRequest(new { mensagem = "Este CPF já esta sendo usado!" });

            _userRepo.CreateUser(model);

            return StatusCode(201, new { mensagem = "Usuário criado!" });
        }
    }
}