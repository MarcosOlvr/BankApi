using BankApi.Repositories.Contracts;
using Desafio.Data;
using Desafio.Models;
using Desafio.Models.DTOs;

namespace BankApi.Repositories
{
    public class TransacaoRepository : ITransacaoRepository
    {
        private readonly AppDbContext _db;

        public TransacaoRepository(AppDbContext db)
        {
            _db = db;
        }

        public Transacao CreateTransacao(TransacaoByCpf model, int userId)
        {
            var enviante = _db.User.Find(userId);
            var recebedor = _db.User.Find(model.Recebedor);
            if (enviante == null || recebedor == null)
                throw new Exception("Usuário não encontrado!");

            var saldo = enviante.SaldoInicial;
            if (saldo <= 0 || saldo < model.Valor)
                throw new Exception("Saldo insuficiente!");

            enviante.SaldoInicial -= model.Valor;
            recebedor.SaldoInicial += model.Valor;

            Transacao transacao = new Transacao()
            {
                ContaRecebedora = recebedor.Id,
                ContaEnviante = userId,
                Valor = model.Valor
            };

            _db.Transacoes.Add(transacao);
            _db.SaveChanges();

            return transacao;
        }

        public Transacao CreateTransacao(TransacaoDTO model, int userId)
        {
            var enviante = _db.User.Find(userId);
            var recebedor = _db.User.Find(model.Recebedor);
            if (enviante == null || recebedor == null)
                throw new Exception("Usuário não encontrado!");

            var saldo = enviante.SaldoInicial;
            if (saldo <= 0 || saldo < model.Valor)
                throw new Exception("Saldo insuficiente!");

            enviante.SaldoInicial -= model.Valor;
            recebedor.SaldoInicial += model.Valor;

            Transacao transacao = new Transacao()
            {
                ContaRecebedora = recebedor.Id,
                ContaEnviante = userId,
                Valor = model.Valor
            };

            _db.Transacoes.Add(transacao);
            _db.SaveChanges();

            return transacao;
        }

        public List<Transacao> GetByDate(DateTime inicial, DateTime final, int userId)
        {
            var transacoes = _db.Transacoes.Where(x => x.ContaEnviante == userId || x.ContaRecebedora == userId).ToList();

            var minhasTransferencias = new List<Transacao>();
            foreach (var obj in transacoes)
            {
                if (obj.DataDeProcessamento >= inicial && obj.DataDeProcessamento <= final)
                {
                    minhasTransferencias.Add(obj);
                }
            }

            return minhasTransferencias;
        }

        public Transacao GetTransacao(int id)
        {
            var transferencia = _db.Transacoes.Find(id);

            return transferencia;
        }

        public void UpdateTransacao(int id)
        {
            var transferencia = _db.Transacoes.Find(id);
            if (transferencia == null)
                throw new Exception("Transação não encontrada!");

            var recebedor = _db.User.Find(transferencia.ContaRecebedora);
            var user = _db.User.Find(transferencia.ContaEnviante);

            if (user == null || recebedor == null)
                throw new Exception("Erro ao procurar um usuário!");

            transferencia.PodeSerEstornada = false;
            recebedor.SaldoInicial -= transferencia.Valor;
            user.SaldoInicial += transferencia.Valor;

            _db.Transacoes.Update(transferencia);
            _db.SaveChanges();
        }
    }
}