using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using budgeting_application.DataAccessLayer.Entities;
using budgeting_application.DataAccessLayer.Setting;
using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace budgeting_application.DataAccessLayer.Repository
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ConnectionSetting? _connection;

        public TransactionRepository(IOptions<ConnectionSetting> connection)
        {
            _connection = connection.Value;
        }

        public void AddTransaction(Transaction transaction)
        {
            using (var connection = new SqlConnection(_connection.SQLString))
            {
                var query = @"INSERT INTO Transactions (Amount, Currency, Category, Date, Type) VALUES (@Amount, @Currency, @Category, @Date, @Type)";
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Amount", transaction.Amount);
                command.Parameters.AddWithValue("@Currency", transaction.Currency);
                command.Parameters.AddWithValue("@Category", transaction.Category);
                command.Parameters.AddWithValue("@Date", transaction.Date);
                command.Parameters.AddWithValue("@Type", transaction.Type);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void DeleteTransaction(int transactionID)
        {
            using (var connection = new SqlConnection(_connection.SQLString))
            {
                var query = @"DELETE FROM Transactions WHERE TransactionID = @TransactionID";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TransactionID", transactionID);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }


        public IEnumerable<Transaction> GetAllTransaction()
        {
            var transactions = new List<Transaction>();
            using (var connection = new SqlConnection(_connection.SQLString))
            {
                var query = @"SELECT * FROM Transactions";
                var command = new SqlCommand(query, connection);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        transactions.Add(new Transaction
                        {
                            TransactionID = reader.GetInt32(0),
                            Amount = reader.GetDecimal(1),
                            Currency = reader.GetString(2),
                            Category = reader.GetString(3),
                            Date = reader.GetDateTime(4),
                            Type = reader.GetString(5)
                        });
                    }
                }
            }
            return transactions;
        }

        public IEnumerable<Transaction> GetTransactionById(int transactionID)
        {
            var transactions = new List<Transaction>();

            using (var connection = new SqlConnection(_connection.SQLString))
            {
                var query = @"SELECT TransactionID, Amount, Currency, Category, Date, Type, UserId 
                      FROM Transactions 
                      WHERE TransactionID = @TransactionID";

                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@TransactionID", transactionID);

                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var transaction = new Transaction
                        {
                            TransactionID = reader.GetInt32(0), 
                            Amount = reader.GetDecimal(1),      
                            Currency = reader.GetString(2),     
                            Category = reader.GetString(3),     
                            Date = reader.GetDateTime(4),        
                            Type = reader.GetString(5),          
                        };

                        transactions.Add(transaction);
                    }
                }
            }

            return transactions;
        }


        public void UpdateTransaction(Transaction transaction)
        {
            using (var connection = new SqlConnection(_connection.SQLString))
            {
                var query = @"UPDATE Transactions 
                      SET Amount = @Amount, 
                          Currency = @Currency, 
                          Category = @Category, 
                          Date = @Date, 
                          Type = @Type 
                      WHERE TransactionID = @TransactionID AND UserId = @UserId";

                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@TransactionID", transaction.TransactionID); 
                command.Parameters.AddWithValue("@Amount", transaction.Amount);
                command.Parameters.AddWithValue("@Currency", transaction.Currency);
                command.Parameters.AddWithValue("@Category", transaction.Category);
                command.Parameters.AddWithValue("@Date", transaction.Date);
                command.Parameters.AddWithValue("@Type", transaction.Type);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

    }

}
