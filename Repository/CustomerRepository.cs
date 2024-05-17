using Assesment.Models;
using Assessment.Models;
using System.Data;
using System.Data.SqlClient;

namespace Assessment.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        SqlConnection _connection = null;
        SqlCommand _command = null;

        public IConfiguration Configuration { get; set; }
        private string GetConnectionString()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");

            Configuration = builder.Build();

            return Configuration.GetConnectionString("DefaultConnection");


        }

        public void CreateCustomer(CustomerModel customer)
        {
            using (_connection = new SqlConnection(GetConnectionString()))
            {
                string query = @"INSERT INTO Customer (UserId, Username, Email, FirstName, LastName, CreatedOn, IsActive) 
                                 VALUES (@UserId, @Username, @Email, @FirstName, @LastName, @CreatedOn, @IsActive)";

                using (SqlCommand command = new SqlCommand(query, _connection))
                {
                    command.Parameters.AddWithValue("@UserId", customer.UserId);
                    command.Parameters.AddWithValue("@Username", customer.Username);
                    command.Parameters.AddWithValue("@Email", customer.Email);
                    command.Parameters.AddWithValue("@FirstName", customer.FirstName);
                    command.Parameters.AddWithValue("@LastName", customer.LastName);
                    command.Parameters.AddWithValue("@CreatedOn", customer.CreatedOn);
                    command.Parameters.AddWithValue("@IsActive", customer.IsActive);

                    try
                    {
                        _connection.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {

                        throw new Exception("An error occurred while creating the customer.", ex);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("An error occurred while creating the customer.", ex);
                    }
                }
            }
        }

        public void DeleteCustomer(Guid id)
        {
            using (_connection = new SqlConnection(GetConnectionString()))
            {
                string query = "DELETE FROM Customer WHERE UserId = @UserId";

                using (SqlCommand command = new SqlCommand(query, _connection))
                {
                    command.Parameters.AddWithValue("@UserId", id);

                    try
                    {
                        _connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected == 0)
                        {
                            Console.WriteLine("Please Check the customer ID");
                        }
                    }
                    catch (SqlException ex)
                    {

                        Console.WriteLine("Customer cannot be Deleted."+ ex.Message);
                    }
                    catch (Exception ex)
                    {

                        Console.WriteLine("Customer cannot be Deleted." + ex.Message);
                    }
                    finally
                    {
                        _connection.Close();
                    }

                }
            }
        }


        public IEnumerable<OrderModel> GetActiveOrdersByCustomer(Guid customerId)
        {
            List<OrderModel> orders = new List<OrderModel>();

            using (_connection = new SqlConnection(GetConnectionString()))
            {
                using (_command = _connection.CreateCommand())
                {
                    try
                    {
                        _command.CommandText = "GetActiveOrders_By_Customer";
                        _command.CommandType = CommandType.StoredProcedure;

                        _command.Parameters.AddWithValue("@CustomerId", customerId);

                        _connection.Open();

                        using (SqlDataReader reader = _command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                OrderModel order = new OrderModel
                                {
                                    OrderId = reader.GetGuid("OrderId"),
                                    OrderStatus = reader.GetInt32("OrderStatus"),
                                    OrderType = reader.GetInt32("OrderType"),
                                    OrderBy = reader.GetGuid("OrderBy"),
                                    OrderedOn = reader.GetDateTime("OrderedOn"),
                                    ShippedOn = reader.GetDateTime("ShippedOn"),
                                    Product = new ProductModel
                                    {
                                        ProductId = reader.GetGuid("ProductId"),
                                        ProductName = reader.GetString("ProductName"),
                                        UnitPrice = reader.GetDecimal("UnitPrice")
                                    },
                                    Supplier = new SupplierModel
                                    {
                                        SupplierId = reader.GetGuid("SupplierId"),
                                        SupplierName = reader.GetString("SupplierName")
                                    }
                                };

                                orders.Add(order);
                            }
                        }
                    }
                    catch (Exception ex) {

                        Console.WriteLine(ex.Message);
                    
                    }
                    finally
                    {
                        _connection.Close();
                    }
                }
            }

            return orders;
        }


        public IEnumerable<CustomerModel> GetAllCustomers()
        {
            List<CustomerModel> customers = new List<CustomerModel>();
            using (_connection = new SqlConnection(GetConnectionString()))
            {
                string query = "SELECT * FROM Customer";
                using (SqlCommand command = new SqlCommand(query, _connection))
                {
                    try
                    {

                        _connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                customers.Add(new CustomerModel
                                {
                                    UserId = reader.GetGuid(reader.GetOrdinal("UserId")),
                                    Username = reader.GetString(reader.GetOrdinal("Username")),
                                    Email = reader.GetString(reader.GetOrdinal("Email")),
                                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                    LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                    CreatedOn = reader.GetDateTime(reader.GetOrdinal("CreatedOn")),
                                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
                                });
                            }
                        }
                    }
                    catch(Exception ex) 
                    {
                        Console.WriteLine(ex.Message);
                    }
                    finally
                    {
                        _connection.Close();
                    }
                }
            }
            return customers;
        }

        public CustomerModel GetCustomerById(Guid id)
        {
            CustomerModel customer = null;
            using (_connection = new SqlConnection(GetConnectionString()))
            {
                string query = "SELECT * FROM Customer WHERE UserId = @UserId";
                using (SqlCommand command = new SqlCommand(query, _connection))
                {
                    command.Parameters.AddWithValue("@UserId", id);
                    try
                    {
                        _connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                customer = new CustomerModel
                                {
                                    UserId = reader.GetGuid(reader.GetOrdinal("UserId")),
                                    Username = reader.GetString(reader.GetOrdinal("Username")),
                                    Email = reader.GetString(reader.GetOrdinal("Email")),
                                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                    LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                    CreatedOn = reader.GetDateTime(reader.GetOrdinal("CreatedOn")),
                                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
                                };
                            }
                        }
                    }
                    catch(Exception ex) 
                    {
                        Console.WriteLine(ex.Message);
                    }
                    finally
                    {
                        _connection.Close();
                    }

                }
            }
            return customer;
        }
            
        
        public void UpdateCustomer(CustomerModel customer)
        {
            using (_connection = new SqlConnection(GetConnectionString()))
            {
                string query = @"UPDATE Customer 
                                 SET Username = @Username, 
                                     Email = @Email, 
                                     FirstName = @FirstName, 
                                     LastName = @LastName, 
                                     IsActive = @IsActive
                                 WHERE UserId = @UserId";

                using ( _command = new SqlCommand(query, _connection))
                {
                    _command.Parameters.AddWithValue("@UserId", customer.UserId);
                    _command.Parameters.AddWithValue("@Username", customer.Username);
                    _command.Parameters.AddWithValue("@Email", customer.Email);
                    _command.Parameters.AddWithValue("@FirstName", customer.FirstName);
                    _command.Parameters.AddWithValue("@LastName", customer.LastName);
                    _command.Parameters.AddWithValue("@IsActive", customer.IsActive);

                    try
                    {
                        _connection.Open();
                        int rowsAffected = _command.ExecuteNonQuery();
                        if (rowsAffected == 0)
                        {
                            throw new InvalidOperationException("No customer found with the specified UserId.");
                        }
                    }
                    catch (Exception ex)
                    {

                        throw new Exception("An error occurred while updating the customer.", ex);
                    }
                    finally
                    {
                        _connection.Close();
                    }
                }
            }
        }
    }
 }
