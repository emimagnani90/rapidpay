using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Transaction
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public decimal Fee { get; set; }
        public DateTime Date { get; set; }
        public CreditCard? CreditCard { get; set; }
    }
}
