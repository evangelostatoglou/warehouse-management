using WM.Application.DTOs;

namespace WM.Application.Services;

public interface ICustomerService
{
    Task<IReadOnlyList<GetCustomerResponse>> GetAllCustomersAsync();
    Task<GetCustomerResponse?> GetCustomerByIdAsync(int id);
    Task<GetCustomerResponse> CreateCustomerAsync(CreateCustomerRequest customer);
    Task<GetCustomerResponse?> UpdateCustomerAsync(UpdateCustomerRequest customer, int id);
    Task<GetCustomerResponse?> UpdateCustomerStatusAsync(UpdateCustomerStatusRequest status, int id);
}
