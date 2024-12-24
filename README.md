# CustomerMVCCore3.1DapperWebApp1

**Startup.cs**

public void ConfigureServices(IServiceCollection services)
<br/>
{
<br/>
    //To includes both APIs and Razor Views<br/> 
    services.AddControllersWithViews();

    services.AddScoped<CustomerRepository>((provider =>
    {
        var configuration = provider.GetRequiredService<IConfiguration>();
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        return new CustomerRepository(connectionString);
    }));
}

<br/>

**CustomerRepository.cs**

public class CustomerRepository
<br/>
{
<br/>
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