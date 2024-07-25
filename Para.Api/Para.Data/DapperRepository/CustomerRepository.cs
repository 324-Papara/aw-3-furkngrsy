using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Para.Data.Domain;


namespace Para.Data.DapperRepository
{
    public class CustomerRepository
    {
        private readonly IConfiguration _configuration;

        public CustomerRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private IDbConnection CreateConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("MsSqlConnection"));
        }

        public async Task<Customer?> GetCustomerWithDetailsAsync(long customerId)
        {
            const string query = @"
                SELECT * FROM Customer WHERE Id = @CustomerId;
                SELECT * FROM CustomerDetail WHERE CustomerId = @CustomerId;
                SELECT * FROM CustomerAddress WHERE CustomerId = @CustomerId;
                SELECT * FROM CustomerPhone WHERE CustomerId = @CustomerId;
            ";

            using (var connection = CreateConnection())
            {
                using (var multi = await connection.QueryMultipleAsync(query, new { CustomerId = customerId }))
                {
                    var customer = await multi.ReadFirstOrDefaultAsync<Customer>();
                    if (customer != null)
                    {
                        customer.CustomerDetail = await multi.ReadFirstOrDefaultAsync<CustomerDetail>();
                        customer.CustomerAddresses = (await multi.ReadAsync<CustomerAddress>()).ToList();
                        customer.CustomerPhones = (await multi.ReadAsync<CustomerPhone>()).ToList();
                    }
                    return customer;
                }
            }
        }
    }
}
