using Microsoft.EntityFrameworkCore;
using Restaurant.Data;
using Restaurant.Interfaces;
using Restaurant.Models;

namespace Restaurant.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ApplicationDbContext _context;

        public CustomerService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CustomerDto>> ListCustomers()
        {
            List<Customer> customers = await _context.Customers
                .Include(c => c.Orders)
                .ThenInclude(o => o.OrderItems)
                .ToListAsync(); // ✅ Convert to List first

            List<CustomerDto> customerDtos = new();

            foreach (Customer customer in customers)
            {
                var lastOrder = customer.Orders.OrderByDescending(o => o.OrderDate).FirstOrDefault();

                customerDtos.Add(new CustomerDto()
                {
                    CustomerId = customer.CustomerId,
                    Name = customer.Name,
                    LastOrderDate = lastOrder != null ? (DateOnly?)lastOrder.OrderDate : null, // ✅ Fixed nullable issue
                    LastOrderPrice = lastOrder != null && lastOrder.OrderItems != null
                                     ? lastOrder.OrderItems.Sum(oi => oi.TotalPrice)
                                     : 0
                });
            }

            return customerDtos;
        }

        public async Task<CustomerDto?> FindCustomer(int id)
        {
            var customer = await _context.Customers
                .Include(c => c.Orders)
                .ThenInclude(o => o.OrderItems)
                .FirstOrDefaultAsync(c => c.CustomerId == id);

            if (customer == null)
            {
                return null;
            }

            var lastOrder = customer.Orders.OrderByDescending(o => o.OrderDate).FirstOrDefault();

            return new CustomerDto()
            {
                CustomerId = customer.CustomerId,
                Name = customer.Name,
                Email = customer.Email ?? "",
                Phone = customer.Phone ?? "",
                LastOrderDate = lastOrder != null ? (DateOnly?)lastOrder.OrderDate : null,
                LastOrderPrice = lastOrder != null && lastOrder.OrderItems != null
                                 ? lastOrder.OrderItems.Sum(oi => oi.TotalPrice)
                                 : 0
            };
        }

        public async Task<ServiceResponse> UpdateCustomer(int id, AUCustomerDto updateCustomerDto)
        {
            ServiceResponse serviceResponse = new();

            if (id != updateCustomerDto.CustomerId)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("Customer ID mismatch.");
                return serviceResponse;
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.NotFound;
                serviceResponse.Messages.Add("Customer not found.");
                return serviceResponse;
            }

            customer.Name = updateCustomerDto.Name;
            customer.Email = updateCustomerDto.Email;
            customer.Phone = updateCustomerDto.Phone;

            _context.Entry(customer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("An error occurred updating the record.");
                return serviceResponse;
            }

            serviceResponse.Status = ServiceResponse.ServiceStatus.Updated;
            return serviceResponse;
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.CustomerId == id);
        }

        public async Task<ServiceResponse> AddCustomer(AUCustomerDto addCustomerDto)
        {
            ServiceResponse serviceResponse = new();

            if (!string.IsNullOrEmpty(addCustomerDto.Email))
            {
                var existingCustomer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == addCustomerDto.Email);
                if (existingCustomer != null)
                {
                    serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                    serviceResponse.Messages.Add("Customer with this email already exists.");
                    return serviceResponse;
                }
            }

            Customer customer = new()
            {
                Name = addCustomerDto.Name,
                Email = addCustomerDto.Email,
                Phone = addCustomerDto.Phone
            };

            try
            {
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();
                serviceResponse.Status = ServiceResponse.ServiceStatus.Created;
                serviceResponse.CreatedId = customer.CustomerId;
            }
            catch (Exception ex)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("There was an error adding the customer: " + ex.Message);
                return serviceResponse;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse> DeleteCustomer(int id)
        {
            ServiceResponse serviceResponse = new();

            var customer = await _context.Customers
                .Include(c => c.Orders) // Load orders associated with the customer
                .FirstOrDefaultAsync(c => c.CustomerId == id);

            if (customer == null)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.NotFound;
                serviceResponse.Messages.Add("Customer not found.");
                return serviceResponse;
            }

            try
            {
                // ✅ Remove customer’s orders before deleting customer
                _context.Orders.RemoveRange(customer.Orders);
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();

                serviceResponse.Status = ServiceResponse.ServiceStatus.Deleted;
            }
            catch (Exception ex)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("Error deleting customer: " + ex.Message);
            }

            return serviceResponse;
        }

    }
}
