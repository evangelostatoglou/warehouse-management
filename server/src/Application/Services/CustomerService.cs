using WM.Application.DTOs;
using WM.Domain.Entities;

namespace WM.Application.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<IReadOnlyList<GetCustomerResponse>> GetAllCustomersAsync()
    {
        var customers = await _customerRepository.GetAllCustomersAsync();

        return customers.Select(customer => new GetCustomerResponse
        {
            Id = customer.Id,
            Name = customer.Name,
            Email = customer.Email,
            Phone = customer.Phone,
            IsActive = customer.IsActive
        }).ToList();
    }

    public async Task<GetCustomerResponse?> GetCustomerByIdAsync(int id)
    {
        var customer = await _customerRepository.GetCustomerByIdAsync(id);
        if(customer is null) return null;

        return new GetCustomerResponse
        {
            Id = customer.Id,
            Name = customer.Name,
            Email = customer.Email,
            Phone = customer.Phone,
            IsActive = customer.IsActive
        };
    }

    public async Task<GetCustomerResponse> CreateCustomerAsync(CreateCustomerRequest customer)
    {
        var c = new Customer(customer.Name, customer.Email, customer.Phone);

        await _customerRepository.CreateCustomerAsync(c);

        return new GetCustomerResponse
        {
            Id = c.Id,
            Name = c.Name,
            Email = c.Email,
            Phone = c.Phone,
            IsActive = c.IsActive
        };
    }

    public async Task<GetCustomerResponse?> UpdateCustomerAsync(UpdateCustomerRequest customer, int id)
    {
        var c = await _customerRepository.GetCustomerByIdAsync(id);
        if(c is null) return null;

        c.Update(customer.Name, customer.Email, customer.Phone);

        await _customerRepository.UpdateCustomerAsync(c);

        return new GetCustomerResponse
        {
            Id = c.Id,
            Name = c.Name,
            Email = c.Email,
            Phone = c.Phone,
            IsActive = c.IsActive
        };
    }

    public async Task<GetCustomerResponse?> UpdateCustomerStatusAsync(UpdateCustomerStatusRequest status, int id)
    {
        if(status.IsActive is null) return null;

        var c = await _customerRepository.GetCustomerByIdAsync(id);
        if(c is null) return null;

        if(status.IsActive == true) c.Activate();
        else c.Deactivate();

        await _customerRepository.UpdateCustomerAsync(c);

        return new GetCustomerResponse
        {
            Id = c.Id,
            Name = c.Name,
            Email = c.Email,
            Phone = c.Phone,
            IsActive = c.IsActive
        };
    }
}
