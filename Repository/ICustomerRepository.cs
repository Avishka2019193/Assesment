using Assessment.Models;

namespace Assessment.Repository
{
    public interface ICustomerRepository
    {
        CustomerModel GetCustomerById(Guid id);
        IEnumerable<CustomerModel> GetAllCustomers();
        void CreateCustomer(CustomerModel customer);
        void UpdateCustomer(CustomerModel customer);
        void DeleteCustomer(Guid id);
        IEnumerable<OrderModel> GetActiveOrdersByCustomer(Guid customerId);
    }
}
