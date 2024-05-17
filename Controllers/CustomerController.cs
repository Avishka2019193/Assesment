using Assessment.Models;
using Assessment.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Assessment.Controllers
{
    

        [Route("api/[controller]")]
        [ApiController]
        public class CustomerController : Controller
        {
            private readonly ICustomerRepository _customerRepository;

            public CustomerController(ICustomerRepository customerRepository)
            {
                _customerRepository = customerRepository;
            }

            [HttpGet("{id}")]
            public IActionResult GetCustomer(Guid id)
            {
                var customer = _customerRepository.GetCustomerById(id);
                if (customer == null)
                {
                    return NotFound();
                }
                return Ok(customer);
            }


        [HttpGet]
        public IActionResult GetAllCustomers()
        {
            var customers = _customerRepository.GetAllCustomers();
            return Ok(customers);
        }

        [HttpPost]
        public IActionResult CreateCustomer([FromBody] CustomerModel customer)
        {
            _customerRepository.CreateCustomer(customer);
            return Ok("Customer created successfully");
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCustomer(Guid id, [FromBody] CustomerModel customer)
        {
            if (_customerRepository.GetCustomerById(id) == null)
            {
                return NotFound();
            }
            customer.UserId = id;
            _customerRepository.UpdateCustomer(customer);
            return Ok("Customer updated successfully");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCustomer(Guid id)
        {
            var existingCustomer = _customerRepository.GetCustomerById(id);
            if (existingCustomer == null)
            {
                return NotFound();
            }
            _customerRepository.DeleteCustomer(id);
            return Ok("Customer deleted successfully");
        }

        [HttpGet("{customerId}/active-orders")]
        public IActionResult GetActiveOrdersByCustomer(Guid customerId)
        {
            var orders = _customerRepository.GetActiveOrdersByCustomer(customerId);
            return Ok(orders);
        }























        
    }
}
