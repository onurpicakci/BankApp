using System.Data;
using DataAccess.Abstract;
using DataAccess.Context;
using Domain.Entity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repository;

public class UserRepository : IUserRepository
{
    private readonly BankAppDbContext _context;

    public UserRepository(BankAppDbContext context)
    {
        _context = context;
    }

    public async Task<User> AddUserAsync(User user)
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
                        command.CommandText = "InsertUser";
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Username", user.Username);
                        command.Parameters.AddWithValue("@Email", user.Password);
                        command.Parameters.AddWithValue("@Password", user.Password);
                        command.Parameters.AddWithValue("@ConfirmPassword", user.ConfirmPassword);
                        command.Parameters.AddWithValue("@Role", user.Role);

                        await command.ExecuteNonQueryAsync();

                        transaction.Commit();
                        return user;
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

    public async Task<User> GetUserAsync(int id)
    {
        using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
        {
            using (var command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "GetUserById";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", id);

                connection.Open();
                var reader = await command.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var user = new User
                        {
                            Id = reader.GetInt32(0),
                            Username = reader.GetString(1),
                            Password = reader.GetString(2),
                            Role = reader.GetString(3)
                        };
                        return user;
                    }
                }

                connection.Close();
                return null;
            }
        }
    }

    public async Task<User> GetUserByUsernameAsync(string username)
    {
        using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
        {
            using (var command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "GetUserByUsername";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Username", username);

                connection.Open();
                var reader = await command.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var user = new User
                        {
                            Id = reader.GetInt32(0),
                            Username = reader.GetString(1),
                            Email = reader.GetString(2),
                            Password = reader.GetString(3),
                            Role = reader.GetString(4)
                        };
                        return user;
                    }
                }

                connection.Close();
                return null;
            }
        }
    }
}