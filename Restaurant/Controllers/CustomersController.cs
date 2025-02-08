using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.DependencyResolver;
using Restaurant.Data;
using Restaurant.Models;

namespace Restaurant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CustomersController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns a list of Customers 
        /// </summary>
        /// <returns>
        /// 200 Ok
        /// List of Customer including ID, Name, Last Order Date and Last Order Price.
        /// </returns>
        /// <example>
        /// GET: api/Customers/List -> [{CustomerId: 1, Name: "Himani", LastOrderDate: "2025-01-01", LastOrderPrice:30},{....},{....}]
        /// </example>
        [HttpGet(template: "List")]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> ListCustomers()
        {
            List<Customer> Customers = await _context.Customers
                .Include(c => c.Orders)
                .ThenInclude(o => o.OrderItems)
                .ToListAsync();

            List<CustomerDto> CustomerDtos = new List<CustomerDto>();

            foreach (Customer customer in Customers)
            {
                var lastOrder = customer.Orders
                    .OrderByDescending(o => o.OrderDate)
                    .FirstOrDefault();

                CustomerDtos.Add(new CustomerDto()
                {
                    CustomerId = customer.CustomerId,
                    Name = customer.Name,
                    LastOrderDate = lastOrder.OrderDate,
                    LastOrderPrice = lastOrder?.OrderItems?.Sum(oi => oi.TotalPrice) ?? 0
                });
            }

            return Ok(CustomerDtos);
        }



        /// <summary>
        /// Return a Customer specified by it's {id}
        /// </summary>
        /// /// <param name="id">Customer's id</param>
        /// <returns>
        /// 200 Ok
        /// CustomerDto : It includes Customer including ID, Name, Last Order Date and Last Order Price.
        /// or
        /// 404 Not Found when there is no Customer for that {id}
        /// </returns>
        /// <example>
        /// GET: api/Customers/Find/1 -> {CustomerId: 1, Name: "Himani", LastOrderDate: "2025-01-01", LastOrderPrice:30}
        [HttpGet(template: "Find/{id}")]
        public async Task<ActionResult<CustomerDto>> FindCustomer(int id)
        {
            var customer = await _context.Customers
                .Include(c => c.Orders)
                .ThenInclude(o => o.OrderItems)
                .FirstOrDefaultAsync(c => c.CustomerId == id);

            if (customer == null)
            {
                return NotFound();
            }

            var lastOrder = customer.Orders
                .OrderByDescending(o => o.OrderDate)
                .FirstOrDefault();

            CustomerDto customerDto = new CustomerDto()
            {
                CustomerId = customer.CustomerId,
                Name = customer.Name,
                LastOrderDate = lastOrder.OrderDate,
                LastOrderPrice = lastOrder?.OrderItems?.Sum(oi => oi.TotalPrice) ?? 0
            };

            return Ok(customerDto);
        }


        /// <summary>
        /// It updates a Customer
        /// </summary>
        /// <param name="id">The ID of the Customer to update</param>
        /// <param name="updateCustomerDto">The required information to update the Customer</param>
        /// <returns>
        /// 400 Bad Request
        /// or
        /// 404 Not Found
        /// or
        /// 204 No Content
        /// </returns>       
        [HttpPut(template: "Update/{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, AUCustomerDto updateCustomerDto)
        {
            if (id != updateCustomerDto.CustomerId)
            {
                return BadRequest("Customer ID mismatch.");
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            // Update necessary fields
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
                if (!CustomerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Adds a Customer to the Customers table
        /// </summary>
        /// <remarks>
        /// Adds a Customer using AUCustomerDto as input, and returns the added Customer details.
        /// </remarks>
        /// <param name="addCustomerDto">The required information to add the Customer</param>
        /// <returns>
        /// 201 Created
        /// or
        /// 400 Bad Request
        /// </returns>
        /// <example>
        /// api/Customers/Add -> Adds a new Customer 
        /// </example>
        [HttpPost(template: "Add")]
        public async Task<ActionResult<Customer>> AddCustomer(AUCustomerDto addCustomerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Customer customer = new Customer()
            {
                Name = addCustomerDto.Name,
                Email = addCustomerDto.Email,
                Phone = addCustomerDto.Phone
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            AUCustomerDto customerDto = new AUCustomerDto()
            {
                CustomerId = customer.CustomerId,
                Name = customer.Name,
                Email = customer.Email,
                Phone = customer.Phone
            };

            return CreatedAtAction(nameof(FindCustomer), new { id = customer.CustomerId }, customerDto);
        }

        /// <summary>
        /// Delete a Customer specified by it's {id}
        /// </summary>
        /// <param name="id">The id of the Customer we want to delete</param>
        /// <returns>
        /// 201 No Content
        /// or
        /// 404 Not Found
        /// </returns>
        /// <example>
        /// api/Customer/Delete/{id} -> Deletes the Customer associated with {id}
        /// </example>
        [HttpDelete(template: "Delete/{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.CustomerId == id);
        }
    }
}