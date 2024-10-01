using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using budgeting_application.DataAccessLayer.Entities;

namespace budgeting_application.DataAccessLayer.Repository
{
    public interface ITransactionRepository
    {
        void AddTransaction(Transaction transaction);
        IEnumerable<Transaction> GetAllTransaction(); 
        IEnumerable<Balance> GetTotalBalance();
        IEnumerable<Breakdown> GetBreakdown();
        void UpdateTransaction(Transaction transaction);
        void DeleteTransaction(int TransactionID);
        IEnumerable<Transaction> GetTransactionById(int TransactionID);
    }

}
