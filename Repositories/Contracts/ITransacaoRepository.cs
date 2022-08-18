using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Desafio.Models;

namespace BankApi.Repositories.Contracts
{
    public interface ITransacaoRepository
    {
        Transacao GetTransacao(int id);
        void UpdateTransacao(Transacao model);
        Transacao CreateTransacao(Tuple<User, User> usuarios, decimal valor);
        bool SalvarTransacao(Transacao model);
    }
}