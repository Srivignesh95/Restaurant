using Microsoft.AspNetCore.Mvc;
using Restaurant.Interfaces;
using Restaurant.Models.ViewModels;
using Restaurant.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Restaurant.Services;
using Microsoft.AspNetCore.Authorization;

namespace Restaurant.Controllers
{
    public class CustomerPageController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly IOrderService _orderService;
        //private readonly IQuizService _quizService;
        // Dependency injection of service interfaces
        public CustomerPageController(ICustomerService CustomerService, IOrderService OrderService)
        {
            _customerService = CustomerService;
            _orderService = OrderService;
            //_quizService = quizService;
        }

        // Show List of Customers on Index page 
        public IActionResult Index()
        {
            return RedirectToAction("List");
        }

        // GET: CustomerPage/ListCustomers
        [HttpGet("ListCustomers")]
        public async Task<IActionResult> List()
        {
            IEnumerable<CustomerDto?> customerDtos = await _customerService.ListCustomers();
            return View(customerDtos);
        }

        // GET: CustomerPage/CustomerDetails/{id}
        [HttpGet("CustomerDetails/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            CustomerDto? customerDto = await _customerService.FindCustomer(id);

            if (customerDto == null)
            {
                return View("Error", new ErrorViewModel() { Errors = ["Could not find Customer"] });
            }
            else
            {
                var orders = await _orderService.ListOrdersByCustomerId(id);

                CustomerDetails customerInfo = new CustomerDetails()
                {
                    Customer = customerDto,
                    Order = orders.ToList()
                };

                return View(customerInfo);
            }
        }
        // GET: CustomerPage/AddCustomer
        [HttpGet("AddCustomer")]
        public IActionResult AddCustomer()
        {
            return View("AddCustomer");
        }

        // POST: CustomerPage/AddCustomer
        [HttpPost("AddCustomer")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCustomer(AUCustomerDto customerDto)
        {
            if (ModelState.IsValid)
            {
                await _customerService.AddCustomer(customerDto);
                return RedirectToAction("List");
            }

            return View("AddCustomer", customerDto);
        }
        // GET: CustomerPage/EditCustomer/{id}
        [HttpGet("Edit/{id}")]
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            CustomerDto? customerDto = await _customerService.FindCustomer(id);

            if (customerDto == null)
            {
                return View("Error", new ErrorViewModel() { Errors = ["Customer not found"] });
            }

            var updateCustomerDto = new AUCustomerDto
            {
                CustomerId = customerDto.CustomerId,
                Name = customerDto.Name,
                Email = customerDto.Email,
                Phone = customerDto.Phone
            };

            return View(updateCustomerDto);
        }

        // POST: CustomerPage/EditCustomer/{id}
        [HttpPost("Edit/{id}")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AUCustomerDto updateCustomerDto)
        {
            if (id != updateCustomerDto.CustomerId)
            {
                return View("Error", new ErrorViewModel() { Errors = ["Invalid customer ID"] });
            }

            if (ModelState.IsValid)
            {
                var serviceResponse = await _customerService.UpdateCustomer(id, updateCustomerDto);

                if (serviceResponse.Status == ServiceResponse.ServiceStatus.Error)
                {
                    return View("Error", new ErrorViewModel() { Errors = serviceResponse.Messages });
                }

                return RedirectToAction("Details", new { id });
            }

            return View(updateCustomerDto);
        }
        // ✅ GET: CustomerPage/DeleteCustomer/{id}
        [HttpGet("DeleteCustomer/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            CustomerDto? customerDto = await _customerService.FindCustomer(id);

            if (customerDto == null)
            {
                return View("Error", new ErrorViewModel { Errors = ["Customer not found"] });
            }

            return View("DeleteCustomer", customerDto); // Ensure this matches the view file name
        }
        // ✅ POST: CustomerPage/DeleteCustomer/{id}
        [HttpPost("DeleteCustomer/{id}")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _customerService.DeleteCustomer(id);

            if (response.Status == ServiceResponse.ServiceStatus.Deleted)
            {
                return RedirectToAction("List");
            }
            else
            {
                return View("Error", new ErrorViewModel { Errors = response.Messages });
            }
        }



    }
}
