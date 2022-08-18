using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Desafio.Models;

namespace BankApi.Repositories.Contracts
{
    public interface IUserRepository
    {
        User GetUser(int id);
        User GetUser(string cpf);
        bool CreateUser(User model);
        User UserLogin(string cpf, string senha);
    }
}