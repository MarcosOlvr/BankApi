using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Desafio.Models;
using Desafio.Models.ViewModels;

namespace BankApi.Repositories.Contracts
{
    public interface ITransacaoRepository
    {
        Transacao GetTransacao(int id);
        void UpdateTransacao(int id);
        Transacao CreateTransacao(TransacaoByCpf model, int userId);
        Transacao CreateTransacao(TransacaoViewModel model, int userId);
        List<Transacao> GetByDate(DateTime inicial, DateTime final, int userId);
    }
}