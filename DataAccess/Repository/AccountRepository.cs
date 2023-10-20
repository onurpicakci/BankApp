using System.Data;
using DataAccess.Abstract;
using DataAccess.Context;
using Domain.Entity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repository;

public class AccountRepository : IAccountRepository
{
    private readonly BankAppDbContext _context;

    public AccountRepository(BankAppDbContext context)
    {
        _context = context;
    }

    public async Task<Account> AddAccountAsync(Account account)
    {
        using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
        {
            await connection.OpenAsync();

            using (var transaction = connection.BeginTransaction())
            {
                using (var command = new SqlCommand())
                {
                    try
                    {
                        command.Connection = connection;
                        command.Transaction = transaction;
                        command.CommandText = "AddAccount";
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@CustomerNo", account.CustomerNo);
                        command.Parameters.AddWithValue("@Balance", account.Balance);
                        command.Parameters.AddWithValue("@AccountType", account.AccountType);
                        command.Parameters.Add(new SqlParameter
                        {
                            ParameterName = "@AccountNumber",
                            DbType = DbType.Int32,
                            Direction = ParameterDirection.Output,
                            Size = 12
                        });

                        await command.ExecuteNonQueryAsync();

                        transaction.Commit();

                        account.AccountNumber = command.Parameters["@AccountNumber"].Value.ToString();
                        return await Task.FromResult(account);
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }

    public async Task<Account> DepositAsync(string accountNumber, decimal amount)
    {
        using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
        {
            await connection.OpenAsync();

            using (var transaction = connection.BeginTransaction())
            {
                using (var command = new SqlCommand())
                {
                    try
                    {
                        command.Connection = connection;
                        command.Transaction = transaction;
                        command.CommandText = "DepositMoney";
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@AccountNumber", accountNumber);
                        command.Parameters.AddWithValue("@Amount", amount);

                        await command.ExecuteNonQueryAsync();

                        transaction.Commit();

                        var account = new Account
                        {
                            AccountNumber = accountNumber,
                        };
                        return await Task.FromResult(account);
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }

    public async Task<Account> GetAccountNoByCustomerNoAsync(string customerNo)
    {
        using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
        {
            using (var command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "GetAccountNumberByCustomerNo";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CustomerNo", customerNo);

                await connection.OpenAsync();

                var result = await command.ExecuteScalarAsync();

                if (result == null || result == DBNull.Value)
                {
                    return null;
                }

                var account = new Account
                {
                    AccountNumber = result.ToString(),
                    CustomerNo = customerNo,
                };

                return account;
            }
        }
    }

    public async Task<Account> GetAccountByAccountNoAsync(string accountNo)
    {
        using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
        {
            using (var command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "GetAccountByNumber";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@AccountNumber", accountNo);

                await connection.OpenAsync();

                var reader = await command.ExecuteReaderAsync();

                if (!reader.HasRows)
                {
                    return null;
                }

                var account = new Account();

                while (await reader.ReadAsync())
                {
                    account.AccountNumber = reader["AccountNumber"].ToString();
                    account.CustomerNo = reader["CustomerNo"].ToString();
                    account.Balance = Convert.ToDecimal(reader["Balance"]);
                    account.AccountType = Convert.ToInt32(reader["AccountType"]);
                }

                return account;
            }
        }
    }

    public async Task<Account> TransferMoneyAsync(string accountNumber, string destinationAccountNumber, decimal amount)
    {
        using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
        {
            await connection.OpenAsync();

            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    using (var command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.Transaction = transaction;
                        command.CommandText = "WithdrawMoney";
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@AccountNumber", accountNumber);
                        command.Parameters.AddWithValue("@Amount", amount);

                        await command.ExecuteNonQueryAsync();
                    }

                    using (var command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.Transaction = transaction;
                        command.CommandText = "DepositMoney";
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@AccountNumber", destinationAccountNumber);
                        command.Parameters.AddWithValue("@Amount", amount);

                        await command.ExecuteNonQueryAsync();
                    }


                    transaction.Commit();

                    var account = new Account
                    {
                        AccountNumber = accountNumber,
                    };
                    return await Task.FromResult(account);
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }

    public async Task<Account> WithdrawAsync(string accountNumber, decimal amount)
    {
        using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
        {
            await connection.OpenAsync();

            using (var transaction = connection.BeginTransaction())
            {
                using (var command = new SqlCommand())
                {
                    try
                    {
                        command.Connection = connection;
                        command.Transaction = transaction;
                        command.CommandText = "WithdrawMoney";
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@AccountNumber", accountNumber);
                        command.Parameters.AddWithValue("@Amount", amount);

                        await command.ExecuteNonQueryAsync();

                        transaction.Commit();

                        var account = new Account
                        {
                            AccountNumber = accountNumber,
                        };
                        return await Task.FromResult(account);
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }

    public async Task<List<Account>> GetAccountsByCustomerNoAsync(string customerNo)
    {
        using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
        {
            using (var command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "GetAccountsByCustomerNo";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CustomerNo", customerNo);

                await connection.OpenAsync();

                var reader = await command.ExecuteReaderAsync();

                if (!reader.HasRows)
                {
                    return null;
                }

                var accounts = new List<Account>();

                while (await reader.ReadAsync())
                {
                    var account = new Account
                    {
                        AccountNumber = reader["AccountNumber"].ToString(),
                        CustomerNo = reader["CustomerNo"].ToString(),
                        Balance = Convert.ToDecimal(reader["Balance"]),
                        AccountType = Convert.ToInt32(reader["AccountType"])
                    };
                    accounts.Add(account);
                }

                return accounts;
            }
        }
    }

    public async Task<List<Account>> GetAccountsByDifferentCustomerNoAsync(string customerNo)
    {
        using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
        {
            using (var command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "GetAccountsByDifferentCustomerNo";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CustomerNo", customerNo);

                await connection.OpenAsync();

                var reader = await command.ExecuteReaderAsync();

                if (!reader.HasRows)
                {
                    return null;
                }

                var accounts = new List<Account>();

                while (await reader.ReadAsync())
                {
                    var account = new Account
                    {
                        AccountNumber = reader["AccountNumber"].ToString(),
                        CustomerNo = reader["CustomerNo"].ToString(),
                        Balance = Convert.ToDecimal(reader["Balance"]),
                        AccountType = Convert.ToInt32(reader["AccountType"])
                    };
                    accounts.Add(account);
                }

                return accounts;
            }
        }
    }
}