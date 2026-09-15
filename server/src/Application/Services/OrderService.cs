using WM.Application.DTOs;
using WM.Domain.Entities;

namespace WM.Application.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IWarehouseRepository _warehouseRepository;
    private readonly IUserRepository _userRepository;

    public OrderService(
        IOrderRepository orderRepository,
        IProductRepository productRepository,
        ICustomerRepository customerRepository,
        IWarehouseRepository warehouseRepository,
        IUserRepository userRepository)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _customerRepository = customerRepository;
        _warehouseRepository = warehouseRepository;
        _userRepository = userRepository;
    }

    public async Task<IReadOnlyList<GetOrderResponse>> GetAllOrdersAsync()
    {
        var orders = await _orderRepository.GetAllOrdersAsync();

        return orders.Select(order => new GetOrderResponse
        {
            Id = order.Id,
            CustomerId = order.CustomerId,
            WarehouseId = order.WarehouseId,
            CreatedBy = order.CreatedBy,
            Status = order.Status,
            CreatedAt = order.CreatedAt,
            TotalAmount = order.TotalAmount,
            Items = order.Items.Select(item => new GetOrderItemResponse
            {
                Id = item.Id,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                TotalPrice = item.Quantity * item.UnitPrice
            }).ToList()
        }).ToList();
    }

    public async Task<GetOrderResponse?> GetOrderByIdAsync(int id)
    {
        var order = await _orderRepository.GetOrderByIdAsync(id);
        if(order is null) return null;

        return new GetOrderResponse
        {
            Id = order.Id,
            CustomerId = order.CustomerId,
            WarehouseId = order.WarehouseId,
            CreatedBy = order.CreatedBy,
            Status = order.Status,
            CreatedAt = order.CreatedAt,
            TotalAmount = order.TotalAmount,
            Items = order.Items.Select(item => new GetOrderItemResponse
            {
                Id = item.Id,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                TotalPrice = item.Quantity * item.UnitPrice
            }).ToList()
        };
    }

    public async Task<GetOrderResponse?> CreateOrderAsync(CreateOrderRequest order, int createdBy)
    {
        var customer = await _customerRepository.GetCustomerByIdAsync(order.CustomerId);
        var warehouse = await _warehouseRepository.GetWarehouseByIdAsync(order.WarehouseId);
        var user = await _userRepository.GetUserByIdAsync(createdBy);

        if(customer is null || warehouse is null || user is null) return null;

        if(!customer.IsActive || !warehouse.IsActive || !user.IsActive)
            throw new ArgumentException("Customer, warehouse, and user must be active.");

        if(order.Items.GroupBy(item => item.ProductId).Any(group => group.Count() > 1))
            throw new ArgumentException("Each product can appear only once in an order.");

        var newOrder = new Order(order.CustomerId, order.WarehouseId, createdBy);

        foreach(var item in order.Items)
        {
            var product = await _productRepository.GetProductByIdAsync(item.ProductId);

            if(product is null) return null;

            if(!product.IsActive)
                throw new ArgumentException("All products in an order must be active.");

            newOrder.AddItem(item.ProductId, item.Quantity, product.Price);
        }

        await _orderRepository.CreateOrderAsync(newOrder);

        return new GetOrderResponse
        {
            Id = newOrder.Id,
            CustomerId = newOrder.CustomerId,
            WarehouseId = newOrder.WarehouseId,
            CreatedBy = newOrder.CreatedBy,
            Status = newOrder.Status,
            CreatedAt = newOrder.CreatedAt,
            TotalAmount = newOrder.TotalAmount,
            Items = newOrder.Items.Select(item => new GetOrderItemResponse
            {
                Id = item.Id,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                TotalPrice = item.Quantity * item.UnitPrice
            }).ToList()
        };
    }

    public async Task<GetOrderResponse?> ConfirmOrderAsync(int id)
    {
        var order = await _orderRepository.ConfirmOrderAsync(id);
        if(order is null) return null;

        return new GetOrderResponse
        {
            Id = order.Id,
            CustomerId = order.CustomerId,
            WarehouseId = order.WarehouseId,
            CreatedBy = order.CreatedBy,
            Status = order.Status,
            CreatedAt = order.CreatedAt,
            TotalAmount = order.TotalAmount,
            Items = order.Items.Select(item => new GetOrderItemResponse
            {
                Id = item.Id,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                TotalPrice = item.Quantity * item.UnitPrice
            }).ToList()
        };
    }

    public async Task<GetOrderResponse?> CancelOrderAsync(int id)
    {
        var order = await _orderRepository.CancelOrderAsync(id);
        if(order is null) return null;
 
        return new GetOrderResponse
        {
            Id = order.Id,
            CustomerId = order.CustomerId,
            WarehouseId = order.WarehouseId,
            CreatedBy = order.CreatedBy,
            Status = order.Status,
            CreatedAt = order.CreatedAt,
            TotalAmount = order.TotalAmount,
            Items = order.Items.Select(item => new GetOrderItemResponse
            {
                Id = item.Id,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                TotalPrice = item.Quantity * item.UnitPrice
            }).ToList()
        };
    }

    public async Task<GetOrderResponse?> ShipOrderAsync(int id)
    {
        var order = await _orderRepository.ShipOrderAsync(id);
        if(order is null) return null;

        return new GetOrderResponse
        {
            Id = order.Id,
            CustomerId = order.CustomerId,
            WarehouseId = order.WarehouseId,
            CreatedBy = order.CreatedBy,
            Status = order.Status,
            CreatedAt = order.CreatedAt,
            TotalAmount = order.TotalAmount,
            Items = order.Items.Select(item => new GetOrderItemResponse
            {
                Id = item.Id,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                TotalPrice = item.Quantity * item.UnitPrice
            }).ToList()
        };
    }
}
