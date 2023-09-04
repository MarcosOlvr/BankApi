using Desafio.Models;
using Desafio.Models.DTOs;

namespace BankApi.Repositories.Contracts
{
    public interface ITransacaoRepository
    {
        Transacao GetTransacao(int id);
        void UpdateTransacao(int id);
        Transacao CreateTransacao(TransacaoByCpf model, int userId);
        Transacao CreateTransacao(TransacaoDTO model, int userId);
        List<Transacao> GetByDate(DateTime inicial, DateTime final, int userId);
    }
}