# Warehouse Management System

A full-stack warehouse and inventory management application, currently in development. It will help a company manage products, warehouses, stock, customers, suppliers, purchase orders, and customer orders.

This is a portfolio project built to show recruiters how I approach a business-focused backend problem: keeping inventory accurate when several employees and customer orders affect the same stock at the same time.

## Why I built it

I am building this project to demonstrate my C# and ASP.NET Core skills alongside PostgreSQL database design, EF Core migrations, REST APIs, authentication, role-based access control, transactions, and concurrency handling.

The React frontend will be developed with AI assistance. My main focus and contribution are the backend architecture, business rules, database design, and the inventory consistency logic.

## Planned features

- Manage products, suppliers, warehouses, customers, and employee users.
- Track stock separately in each warehouse.
- Create customer orders and reserve available stock before shipping.
- Prevent overselling when two requests try to buy the final units at the same time.
- Ship and cancel orders while keeping stock reservations correct.
- Transfer stock between warehouses in one database transaction.
- Create purchase orders and receive goods from suppliers.
- Keep a stock-movement history for purchases, sales, returns, transfers, and adjustments.
- Support employee roles: Admin, Warehouse, Sales, and Viewer.
- Record important actions in an audit log.
- Show low-stock alerts through a background service.

## Inventory logic

Each product can exist in many warehouses. For every product and warehouse pair, the system will store:

```text
quantity_on_hand
quantity_reserved
available stock = quantity_on_hand - quantity_reserved
```

When an order is confirmed, the requested quantity is reserved. When it is shipped, physical stock is reduced and the reservation is removed. If a step fails, the database transaction will roll back so the inventory remains consistent.

## Technology

| Part | Planned tools |
| --- | --- |
| Backend | C#, ASP.NET Core Web API |
| Database | PostgreSQL |
| Data access | Entity Framework Core |
| Migrations | EF Core Migrations |
| Frontend | React, TypeScript, Vite |
| Authentication | JWT and role-based authorization |
| Caching | Redis, later in the project if needed |
| Testing | xUnit and ASP.NET Core integration testing |
| Containers | Docker Compose, added near the end of the project |

## Planned backend structure

```text
server/
└── src/
    ├── Api/             HTTP controllers, configuration, and dependency injection
    ├── Application/     Services, DTOs, and interfaces
    ├── Domain/          Business entities and rules
    └── Infrastructure/  EF Core, PostgreSQL, repositories, and migrations
```

Requests will normally follow:

```text
Controller → Service → Repository / EF Core → PostgreSQL
```

## Main data areas

| Area | Purpose |
| --- | --- |
| Products | Product catalogue, SKU, price, and active status. |
| Warehouses | Physical locations where stock is stored. |
| Inventory | Current on-hand and reserved stock per product and warehouse. |
| Orders | Customer orders, order items, reservations, shipping, and cancellation. |
| Suppliers and purchasing | Supplier records, purchase orders, and received stock. |
| Stock movements | An auditable history of every inventory change. |
| Users and roles | Employee accounts and authorization rules. |

## Current status

The project structure and initial database entities are being built. Product and warehouse API work has started. The remaining features above are planned and will be added incrementally.
