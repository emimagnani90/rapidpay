using DataAccess.Repositories.Interfaces;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class TransactionRepository : BaseRepository, ITransactionRepository
    {
        public TransactionRepository(ApplicationContext context) : base(context)
        {

        }
        public Transaction AddTransaction(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            return transaction;
        }
    }
}
