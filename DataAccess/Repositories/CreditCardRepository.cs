using DataAccess.Repositories.Interfaces;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class CreditCardRepository : BaseRepository, ICreditCardRepository
    {
        public CreditCardRepository(ApplicationContext context) : base(context)
        {

        }
        public CreditCard AddCreditCard(CreditCard creditCard)
        {
            _context.CreditCards.Add(creditCard);
            return creditCard;
        }

        public CreditCard? GetByNumber(string number)
        {
            var creditCard = _context.CreditCards.Where(x => x.Number == number).FirstOrDefault();
            return creditCard;
        }

        public CreditCard Update(CreditCard creditCard)
        {
            _context.CreditCards.Update(creditCard);
            return creditCard;
        }
    }
}
