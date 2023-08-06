using Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class UFEService : IUFEService
    {
        private decimal _fee;
        private Random _random;

        public UFEService()
        {
            _random = new Random();
            UpdateFee();
        }

        public decimal CalculateTransactionFee(decimal transactionAmount)
        {
            return transactionAmount * _fee / 100;
        }

        public void UpdateFee()
        {
            decimal newDecimal = 0;
            // -- In case new decimal is 0
            // -- Prevent to get stuck with this value
            while (newDecimal == 0)
            {
                newDecimal = GenerateNewDecimal();
            }
            _fee = _fee != 0 ? _fee * newDecimal : newDecimal;
        }

        private decimal GenerateNewDecimal()
        {
            // -- Multiple for 2 because NextDouble generate a value between 0 and 1
            return new decimal(_random.NextDouble() * 2);
        }
    }
}
