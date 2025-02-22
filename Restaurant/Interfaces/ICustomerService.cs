using Restaurant.Models;

namespace Restaurant.Interfaces
{
    public interface ICustomerService
    {
        Task<IEnumerable<CustomerDto>> ListCustomers();
        Task<CustomerDto?> FindCustomer(int id);
        Task<ServiceResponse> AddCustomer(AUCustomerDto addCustomerDto);
        Task<ServiceResponse> UpdateCustomer(int id, AUCustomerDto updateCustomerDto);
        Task<ServiceResponse> DeleteCustomer(int id);
    }
}
