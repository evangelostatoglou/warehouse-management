using WM.Domain.Entities;

namespace WM.Application.Services;

public interface ICustomerRepository
{
    Task<IReadOnlyList<Customer>> GetAllCustomersAsync();
    Task<Customer?> GetCustomerByIdAsync(int id);
    Task CreateCustomerAsync(Customer customer);
    Task UpdateCustomerAsync(Customer customer);
}
