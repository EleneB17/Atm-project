using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atm_project.Info
{
    internal class CardInfo
    {
        public string CardNumber { get; set; }
        public string ExpirationDate { get; set; }
        public string CVC { get; set; }
        public string PinCode { get; set; }
        public decimal Balance { get; set; } = 0;
        public List<TransactionInfo> TransactionHistory { get; set; } = new List<TransactionInfo>();
    }
}

