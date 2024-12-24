using CustomerMVCCore31DapperWebApp1.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace CustomerMVCCore31DapperWebApp1.Repositories
{
    //A
    public class CustomerRepository
    {
        private readonly string _connectionString;

        public CustomerRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return await connection.QueryAsync<Customer>("SELECT * FROM Customer");
            }
        }

        public async Task<Customer> GetCustomerByIdAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return await connection.QueryFirstOrDefaultAsync<Customer>(
                    "SELECT * FROM Customer WHERE Id = @Id", new { Id = id });
            }
        }

        public async Task<int> AddCustomerAsync(Customer customer)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = @"INSERT INTO Customer (FirstName, MiddleName, LastName, Address, Email, Phone) 
                        VALUES (@FirstName, @MiddleName, @LastName, @Address, @Email, @Phone)";
                return await connection.ExecuteAsync(sql, customer);
            }
        }

        public async Task<int> UpdateCustomerAsync(Customer customer)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = @"UPDATE Customer 
                        SET FirstName = @FirstName, MiddleName = @MiddleName, LastName = @LastName, Address = @Address, Email=@Email, Phone=@Phone
                        WHERE Id = @Id";
                return await connection.ExecuteAsync(sql, customer);
            }
        }

        public async Task<int> DeleteCustomerAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return await connection.ExecuteAsync("DELETE FROM Customer WHERE Id = @Id", new { Id = id });
            }
        }
    }

    //B
    /*public class CustomerRepository
    {
        private readonly IDbConnection _dbConnection;

        public CustomerRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            const string query = "SELECT * FROM Customer";
            return await _dbConnection.QueryAsync<Customer>(query);
        }

        public async Task<Customer> GetCustomerByIdAsync(int id)
        {
            const string query = "SELECT * FROM Customer WHERE Id = @Id";
            return await _dbConnection.QueryFirstOrDefaultAsync<Customer>(query, new { Id = id });
        }

        public async Task<int> AddCustomerAsync(Customer customer)
        {
            const string query = @"INSERT INTO Customer (FirstName, MiddleName, LastName, Address, Email, Phone) 
                        VALUES (@FirstName, @MiddleName, @LastName, @Address, @Email, @Phone)";
            return await _dbConnection.ExecuteAsync(query, customer);
        }

        public async Task<int> UpdateCustomerAsync(Customer customer)
        {
            const string query = @"UPDATE Customer 
                        SET FirstName = @FirstName, MiddleName=@MiddleName, LastName = @LastName, Address=@Address, Email=@Email, Phone=@Phone
                        WHERE Id = @Id";
            return await _dbConnection.ExecuteAsync(query, customer);
        }

        public async Task<int> DeleteCustomerAsync(int id)
        {
            const string query = "DELETE FROM Customer WHERE Id = @Id";
            return await _dbConnection.ExecuteAsync(query, new { Id = id });
        }
    }*/
}
