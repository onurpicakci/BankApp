using System.Data;
using DataAccess.Abstract;
using DataAccess.Context;
using Domain.Entity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repository;
public class AccountActivitiesRepository : IAccountActivitiesRepository
{
    private readonly BankAppDbContext _context;

    public AccountActivitiesRepository(BankAppDbContext context)
    {
        _context = context;
    }

    public async Task<List<AccountActivities>> GetAccountActivitiesAsync(string accountNumber)
    {
        using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
        {
            using (var command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "GetAccountActivitiesByNumber";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@AccountNumber", accountNumber);

                connection.Open();
                var reader = await command.ExecuteReaderAsync();
                var accountActivities = new AccountActivities();
                var accountActivitiesList = new List<AccountActivities>();
                while (reader.Read())
                {
                    accountActivities = new AccountActivities
                    {
                        Id = reader.GetInt32("Id"),
                        AccountNumber = reader["AccountNumber"].ToString(),
                        ProcessName = reader["ProcessName"].ToString(),
                        Description = reader["Description"].ToString(),
                        OperationDate = Convert.ToDateTime(reader["OperationDate"]),
                    };
                    accountActivitiesList.Add(accountActivities);
                }
                
                return await Task.FromResult(accountActivitiesList);
            }
        }
    }

    public async Task<List<AccountActivities>> GetLastThreeActivitiesAsync(string accountNumber)
    {
        using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
        {
            using (var command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "GetLastThreeAccountActivities";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@AccountNumber", accountNumber);

                connection.Open();
                var reader = await command.ExecuteReaderAsync();
                var accountActivities = new AccountActivities();
                var accountActivitiesList = new List<AccountActivities>();
                while (reader.Read())
                {
                    accountActivities = new AccountActivities
                    {
                        AccountNumber = reader["AccountNumber"].ToString(),
                        ProcessName = reader["ProcessName"].ToString(),
                        Description = reader["Description"].ToString(),
                        OperationDate = Convert.ToDateTime(reader["OperationDate"]),
                    };
                    accountActivitiesList.Add(accountActivities);
                }
                
                return await Task.FromResult(accountActivitiesList);
            }
        }
    }
}