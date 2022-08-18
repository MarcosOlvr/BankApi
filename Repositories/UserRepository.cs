using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankApi.Repositories.Contracts;
using Desafio.Data;
using Desafio.Models;

namespace BankApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _db;

        public UserRepository(AppDbContext db)
        {
            _db = db;
        }

        public bool CreateUser(User model)
        {
            if (model == null)
                return false;

            _db.User.Add(model);
            _db.SaveChanges();

            return true;
        }

        public User GetUser(int id)
        {
            var user = _db.User.Find(id);
            return user;
        }

        public User GetUser(string cpf)
        {
            var user = _db.User.FirstOrDefault(x => x.Cpf == cpf);
            return user;
        }

        public User UserLogin(string cpf, string senha)
        {
            var user = _db.User.FirstOrDefault(x => x.Cpf == cpf && x.Senha == senha);

            if (user == null)
                return null;

            return user;
        }
    }
}