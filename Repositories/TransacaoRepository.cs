using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankApi.Repositories.Contracts;
using Desafio.Data;
using Desafio.Models;

namespace BankApi.Repositories
{
    public class TransacaoRepository : ITransacaoRepository
    {
        private readonly AppDbContext _db;

        public TransacaoRepository(AppDbContext db)
        {
            _db = db;
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

        public Transacao GetTransacao(int id)
        {
            throw new NotImplementedException();
        }

        public bool SalvarTransacao(Transacao model)
        {
            if (model == null)
                return false;

            _db.Transacoes.Add(model);
            _db.SaveChanges();

            return true;
        }

        public void UpdateTransacao(Transacao model)
        {
            _db.Transacoes.Update(model);
            _db.SaveChanges();
        }
    }
}