using DataAccess.Repositories;
using DataAccess.Repositories.Interfaces;
using DataAccess.UnitOfWork.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationContext _context;
        public UnitOfWork()
        {
            _context = new ApplicationContext();
            User = new UserRepository(_context);
            CreditCard = new CreditCardRepository(_context);
            Transaction = new TransactionRepository(_context);
        }
        public IUserRepository User { get; private set; }
        public ICreditCardRepository CreditCard { get; private set; }
        public ITransactionRepository Transaction { get; private set; }
        public int Complete()
        {
            return _context.SaveChanges();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
