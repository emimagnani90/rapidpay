using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Interfaces
{
    public interface ICreditCardRepository
    {
        CreditCard AddCreditCard(CreditCard creditCard);
        CreditCard? GetByNumber(string number);
        CreditCard Update(CreditCard creditCard);
    }
}
