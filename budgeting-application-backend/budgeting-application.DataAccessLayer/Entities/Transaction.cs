using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace budgeting_application.DataAccessLayer.Entities
{
    public class Transaction
    {
        public int TransactionID { get; set; }
        public decimal Amount { get; set; }
        public string? Currency { get; set; }
        public string Currencytype { get; set; }
        public string? Category { get; set; }
        public DateTime Date { get; set; }
        public string? Type { get; set; }
        public decimal totalFiat { get; set; }
        public decimal totalCrypto { get; set; }
    }

}
