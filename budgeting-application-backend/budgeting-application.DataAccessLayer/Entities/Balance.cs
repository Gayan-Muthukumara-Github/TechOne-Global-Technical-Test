using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace budgeting_application.DataAccessLayer.Entities
{
    public class Balance
    {
        public decimal totalFiat { get; set; }
        public decimal totalCrypto { get; set; }
    }
}
