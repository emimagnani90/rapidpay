using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class CreditCard
    {
        public int Id { get; set; }
        public string? Number { get; set; }
        public decimal Balance { get; set; }
    }
}
