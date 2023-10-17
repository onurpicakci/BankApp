using System.Data;
using DataAccess.Abstract;
using DataAccess.Context;
using Domain.Entity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repository;
public class CustomerRepository : ICustomerRepository
{
    private readonly BankAppDbContext _context;

    public CustomerRepository(BankAppDbContext context)
    {
        _context = context;
    }

    public async Task<Customer> AddCustomerAsync(Customer customer)
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
                        command.CommandText = "AddCustomer";
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@TCKN", customer.TCKN);
                        command.Parameters.AddWithValue("@Name", customer.Name);
                        command.Parameters.AddWithValue("@Surname", customer.Surname);
                        command.Parameters.AddWithValue("@Address", customer.Address);
                        command.Parameters.AddWithValue("@Birthdate", customer.Birthdate);
                        command.Parameters.Add(new SqlParameter
                        {
                            ParameterName = "@CustomerNo",
                            DbType = DbType.Int32,
                            Direction = ParameterDirection.Output,
                            Size = 12
                        });

                        await command.ExecuteNonQueryAsync();

                        transaction.Commit();

                        customer.CustomerNo = command.Parameters["@CustomerNo"].Value.ToString();
                        return customer;
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

    public async Task<Customer> GetCustomerByTCKNAsync(string tckn)
    {
        using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
        {
            using (var command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "GetCustomerByTCKN";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TCKN", tckn);

                connection.Open();
                var reader = command.ExecuteReader();
                if (!reader.HasRows) return await Task.FromResult<Customer>(null);
                reader.Read();
                var customer = new Customer
                {
                    CustomerNo = reader["CustomerNo"].ToString(),
                    TCKN = reader["TCKN"].ToString(),
                    Name = reader["Name"].ToString(),
                    Surname = reader["Surname"].ToString(),
                    Address = reader["Address"].ToString(),
                    Birthdate = Convert.ToDateTime(reader["Birthdate"])
                };
                connection.Close();
                return await Task.FromResult(customer);
            }
        }
    }

    public async Task<List<Customer>> GetAllCustomersAsync()
    {
        using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
        {
            using (var command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "GetAllCustomers";
                command.CommandType = CommandType.StoredProcedure;

                connection.Open();
                var reader = command.ExecuteReader();
                if (!reader.HasRows) return await Task.FromResult<List<Customer>>(null);
                var customers = new List<Customer>();
                while (reader.Read())
                {
                    var customer = new Customer
                    {
                        CustomerNo = reader["CustomerNo"].ToString(),
                        TCKN = reader["TCKN"].ToString(),
                        Name = reader["Name"].ToString(),
                        Surname = reader["Surname"].ToString(),
                        Address = reader["Address"].ToString(),
                        Birthdate = Convert.ToDateTime(reader["Birthdate"])
                    };
                    customers.Add(customer);
                }

                connection.Close();
                return await Task.FromResult(customers);
            }
        }
    }
}