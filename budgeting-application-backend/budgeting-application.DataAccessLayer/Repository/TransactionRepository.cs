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
                var query = @"INSERT INTO Transactions (Amount, Currency, Category, Date, Type, Currencytype) VALUES (@Amount, @Currency, @Category, @Date, @Type, @Currencytype)";
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Amount", transaction.Amount);
                command.Parameters.AddWithValue("@Currency", transaction.Currency);
                command.Parameters.AddWithValue("@Category", transaction.Category);
                command.Parameters.AddWithValue("@Date", transaction.Date);
                command.Parameters.AddWithValue("@Type", transaction.Type);
                command.Parameters.AddWithValue("@Currencytype", transaction.Currencytype);
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
                            Type = reader.GetString(5),
                            Currencytype = reader.GetString(6)
                        });
                    }
                }
            }
            return transactions;
        }

        public IEnumerable<Balance> GetTotalBalance()
        {
            var Balance = new List<Balance>();
            using (var connection = new SqlConnection(_connection.SQLString))
            {
                var query = @"SELECT 
                    SUM(CASE WHEN Currencytype = 'Fiat' AND Type = 'Income' THEN Amount ELSE 0 END) AS TotalFiatIncome,
                    SUM(CASE WHEN Currencytype = 'Fiat' AND Type = 'Expense' THEN Amount ELSE 0 END) AS TotalFiatExpenses,
                    SUM(CASE WHEN Currencytype = 'Crypto' AND Type = 'Income' THEN Amount ELSE 0 END) AS TotalCryptoIncome,
                    SUM(CASE WHEN Currencytype = 'Crypto' AND Type = 'Expense' THEN Amount ELSE 0 END) AS TotalCryptoExpenses
                FROM 
                    Transactions";
                var command = new SqlCommand(query, connection);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        decimal totalFiatIncome = reader.IsDBNull(0) ? 0 : reader.GetDecimal(0);
                        decimal totalFiatExpenses = reader.IsDBNull(1) ? 0 : reader.GetDecimal(1);
                        decimal totalCryptoIncome = reader.IsDBNull(2) ? 0 : reader.GetDecimal(2);
                        decimal totalCryptoExpenses = reader.IsDBNull(3) ? 0 : reader.GetDecimal(3);

                        Balance.Add(new Balance
                        {
                            totalFiat = totalFiatIncome - totalFiatExpenses,
                            totalCrypto = totalCryptoIncome - totalCryptoExpenses
                        });
                    }
                }
            }
            return Balance;
        }

        public IEnumerable<Breakdown> GetBreakdown()
        {
            var breakdownData = new List<Breakdown>();

            var query = @"
        SELECT Category, Currency, SUM(Amount) AS TotalAmount, Type
        FROM Transactions
        GROUP BY Category, Currency, Type
    ";

            using (var connection = new SqlConnection(_connection.SQLString))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var breakdown = new Breakdown
                            {
                                Category = reader["Category"].ToString(),
                                Currency = reader["Currency"].ToString(),
                                TotalAmount = Convert.ToDecimal(reader["TotalAmount"]),
                                Type = reader["Type"].ToString()
                            };
                            breakdownData.Add(breakdown);

                        }
                    }
                }
            }

            return breakdownData;
        }

        public IEnumerable<Transaction> GetTransactionById(int transactionID)
        {
            var transactions = new List<Transaction>();

            using (var connection = new SqlConnection(_connection.SQLString))
            {
                var query = @"SELECT TransactionID, Amount, Currency, Category, Date, Type, Currencytype
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
                            Currencytype = reader.GetString(6)
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
                          Type = @Type,
                          Currencytype = @Currencytype
                      WHERE TransactionID = @TransactionID";

                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@TransactionID", transaction.TransactionID); 
                command.Parameters.AddWithValue("@Amount", transaction.Amount);
                command.Parameters.AddWithValue("@Currency", transaction.Currency);
                command.Parameters.AddWithValue("@Category", transaction.Category);
                command.Parameters.AddWithValue("@Date", transaction.Date);
                command.Parameters.AddWithValue("@Type", transaction.Type);
                command.Parameters.AddWithValue("@Currencytype", transaction.Currencytype);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

    }

}
