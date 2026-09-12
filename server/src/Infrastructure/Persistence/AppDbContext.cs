using Microsoft.EntityFrameworkCore;
using WM.Domain.Entities;

namespace WM.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Warehouse> Warehouses => Set<Warehouse>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<Inventory> Inventory => Set<Inventory>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<StockMovement> StockMovements => Set<StockMovement>();
    public DbSet<PurchaseOrder> PurchaseOrders => Set<PurchaseOrder>();
    public DbSet<PurchaseOrderItem> PurchaseOrderItems => Set<PurchaseOrderItem>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ////////////User/////////////
        modelBuilder.Entity<User>(user =>
        {
            user.HasKey(u => u.Id);

            user.Property(u => u.Id)
                .ValueGeneratedOnAdd();

            user.Property(u => u.Name)
                .HasMaxLength(150)
                .IsRequired();

            user.Property(u => u.Email)
                .HasMaxLength(255)
                .IsRequired();

            user.HasIndex(u => u.Email)
                .IsUnique();

            user.Property(u => u.PasswordHash)
                .HasMaxLength(500)
                .IsRequired();

            user.Property(u => u.Role)
                .HasColumnType("character(1)")
                .IsRequired();

            user.Property(u => u.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            user.Property(u => u.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            user.ToTable(table =>
            {
                table.HasCheckConstraint(
                    "CK_Users_Role_Allowed",
                    "\"Role\" IN ('A', 'W', 'S', 'V')");
            });
        });

        ////////////Customer/////////////
        modelBuilder.Entity<Customer>(customer =>
        {
            customer.HasKey(c => c.Id);

            customer.Property(c => c.Id)
                .ValueGeneratedOnAdd();

            customer.Property(c => c.Name)
                .HasMaxLength(150)
                .IsRequired();

            customer.Property(c => c.Email)
                .HasMaxLength(255)
                .IsRequired();

            customer.HasIndex(c => c.Email)
                .IsUnique();

            customer.Property(c => c.Phone)
                .HasMaxLength(50);

            customer.Property(c => c.IsActive)
                .IsRequired()
                .HasDefaultValue(true);
        });

        ////////////Supplier/////////////
        modelBuilder.Entity<Supplier>(supplier =>
        {
            supplier.HasKey(s => s.Id);

            supplier.Property(s => s.Id)
                .ValueGeneratedOnAdd();

            supplier.Property(s => s.Name)
                .HasMaxLength(150)
                .IsRequired();

            supplier.Property(s => s.Email)
                .HasMaxLength(255)
                .IsRequired();

            supplier.HasIndex(s => s.Email)
                .IsUnique();

            supplier.Property(s => s.IsActive)
                .IsRequired()
                .HasDefaultValue(true);
        });

        ////////////Product/////////////
        modelBuilder.Entity<Product>(product =>
        {
            product.HasKey(p => p.Id);
            product.Property(p => p.Id).ValueGeneratedOnAdd();

            product.Property(p => p.Sku).IsRequired().HasMaxLength(50);
            product.HasIndex(p => p.Sku).IsUnique();

            product.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(200);

            product.Property(p => p.Description)
                .HasMaxLength(1000);

            product.Property(p => p.Price)
                .HasPrecision(12, 2);

            product.Property(p => p.ReorderLevel)
                .IsRequired()
                .HasDefaultValue(0);

            product.ToTable(table =>
            {
                table.HasCheckConstraint(
                    "CK_Products_Price_NonNegative",
                    "\"Price\" >= 0");
                table.HasCheckConstraint(
                    "CK_Products_ReorderLevel_NonNegative",
                    "\"ReorderLevel\" >= 0");
            });

            product.Property(p => p.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            product.Property(p => p.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        ////////////Warehouse/////////////
        modelBuilder.Entity<Warehouse>(warehouse =>
        {
            warehouse.HasKey(w => w.Id);

            warehouse.Property(w => w.Id)
                .ValueGeneratedOnAdd();

            warehouse.Property(w => w.Name)
                .HasMaxLength(150)
                .IsRequired();

            warehouse.Property(w => w.Location)
                .HasMaxLength(255)
                .IsRequired();

            warehouse.Property(w => w.IsActive)
                .IsRequired()
                .HasDefaultValue(true);
        });

        ////////////Inventory/////////////
        modelBuilder.Entity<Inventory>(inventory =>
        {
            inventory.HasKey(i => i.Id);

            inventory.Property(i => i.Id)
                .ValueGeneratedOnAdd();

            inventory.Property(i => i.QuantityOnHand)
                .IsRequired()
                .HasDefaultValue(0);

            inventory.Property(i => i.QuantityReserved)
                .IsRequired()
                .HasDefaultValue(0);

            inventory.HasIndex(i => new { i.ProductId, i.WarehouseId })
                .IsUnique();

            inventory.HasOne<Product>()
                .WithMany()
                .HasForeignKey(i => i.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            inventory.HasOne<Warehouse>()
                .WithMany()
                .HasForeignKey(i => i.WarehouseId)
                .OnDelete(DeleteBehavior.Restrict);

            inventory.ToTable(table =>
            {
                table.HasCheckConstraint(
                    "CK_Inventory_QuantityOnHand_NonNegative",
                    "\"QuantityOnHand\" >= 0");
                table.HasCheckConstraint(
                    "CK_Inventory_QuantityReserved_NonNegative",
                    "\"QuantityReserved\" >= 0");
                table.HasCheckConstraint(
                    "CK_Inventory_QuantityReserved_WithinOnHand",
                    "\"QuantityReserved\" <= \"QuantityOnHand\"");
            });
        });

        ////////////Order/////////////
        modelBuilder.Entity<Order>(order =>
        {
            order.HasKey(o => o.Id);

            order.Property(o => o.Id)
                .ValueGeneratedOnAdd();

            order.Property(o => o.Status)
                .HasMaxLength(20)
                .IsRequired()
                .HasDefaultValue("DRAFT");

            order.Property(o => o.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            order.Property(o => o.TotalAmount)
                .HasPrecision(12, 2)
                .IsRequired()
                .HasDefaultValue(0);

            order.HasOne<Customer>()
                .WithMany()
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            order.HasOne<User>()
                .WithMany()
                .HasForeignKey(o => o.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            order.ToTable(table =>
            {
                table.HasCheckConstraint(
                    "CK_Orders_Status_Allowed",
                    "\"Status\" IN ('DRAFT', 'CONFIRMED', 'PICKING', 'SHIPPED', 'CANCELLED')");
                table.HasCheckConstraint(
                    "CK_Orders_TotalAmount_NonNegative",
                    "\"TotalAmount\" >= 0");
            });
        });

        ////////////OrderItem/////////////
        modelBuilder.Entity<OrderItem>(orderItem =>
        {
            orderItem.HasKey(oi => oi.Id);

            orderItem.Property(oi => oi.Id)
                .ValueGeneratedOnAdd();

            orderItem.Property(oi => oi.UnitPrice)
                .HasPrecision(12, 2)
                .IsRequired();

            orderItem.HasIndex(oi => new { oi.OrderId, oi.ProductId })
                .IsUnique();

            orderItem.HasOne<Order>()
                .WithMany()
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            orderItem.HasOne<Product>()
                .WithMany()
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            orderItem.ToTable(table =>
            {
                table.HasCheckConstraint(
                    "CK_OrderItems_Quantity_Positive",
                    "\"Quantity\" > 0");
                table.HasCheckConstraint(
                    "CK_OrderItems_UnitPrice_NonNegative",
                    "\"UnitPrice\" >= 0");
            });
        });

        ////////////StockMovement/////////////
        modelBuilder.Entity<StockMovement>(stockMovement =>
        {
            stockMovement.HasKey(sm => sm.Id);

            stockMovement.Property(sm => sm.Id)
                .ValueGeneratedOnAdd();

            stockMovement.Property(sm => sm.Type)
                .HasMaxLength(20)
                .IsRequired();

            stockMovement.Property(sm => sm.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            stockMovement.HasOne<Product>()
                .WithMany()
                .HasForeignKey(sm => sm.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            stockMovement.HasOne<Warehouse>()
                .WithMany()
                .HasForeignKey(sm => sm.WarehouseId)
                .OnDelete(DeleteBehavior.Restrict);

            stockMovement.HasOne<User>()
                .WithMany()
                .HasForeignKey(sm => sm.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            stockMovement.ToTable(table =>
            {
                table.HasCheckConstraint(
                    "CK_StockMovements_Type_Allowed",
                    "\"Type\" IN ('PURCHASE', 'SALE', 'RETURN', 'TRANSFER_IN', 'TRANSFER_OUT', 'ADJUSTMENT')");
                table.HasCheckConstraint(
                    "CK_StockMovements_Quantity_Positive",
                    "\"Quantity\" > 0");
            });
        });

        ////////////PurchaseOrder/////////////
        modelBuilder.Entity<PurchaseOrder>(purchaseOrder =>
        {
            purchaseOrder.HasKey(po => po.Id);

            purchaseOrder.Property(po => po.Id)
                .ValueGeneratedOnAdd();

            purchaseOrder.Property(po => po.Status)
                .HasMaxLength(25)
                .IsRequired()
                .HasDefaultValue("DRAFT");

            purchaseOrder.Property(po => po.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            purchaseOrder.HasOne<Supplier>()
                .WithMany()
                .HasForeignKey(po => po.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);

            purchaseOrder.HasOne<Warehouse>()
                .WithMany()
                .HasForeignKey(po => po.WarehouseId)
                .OnDelete(DeleteBehavior.Restrict);

            purchaseOrder.HasOne<User>()
                .WithMany()
                .HasForeignKey(po => po.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            purchaseOrder.ToTable(table =>
            {
                table.HasCheckConstraint(
                    "CK_PurchaseOrders_Status_Allowed",
                    "\"Status\" IN ('DRAFT', 'ORDERED', 'PARTIALLY_RECEIVED', 'RECEIVED', 'CANCELLED')");
            });
        });

        ////////////PurchaseOrderItem/////////////
        modelBuilder.Entity<PurchaseOrderItem>(purchaseOrderItem =>
        {
            purchaseOrderItem.HasKey(poi => poi.Id);

            purchaseOrderItem.Property(poi => poi.Id)
                .ValueGeneratedOnAdd();

            purchaseOrderItem.Property(poi => poi.UnitCost)
                .HasPrecision(12, 2)
                .IsRequired();

            purchaseOrderItem.HasIndex(poi => new { poi.PurchaseOrderId, poi.ProductId })
                .IsUnique();

            purchaseOrderItem.HasOne<PurchaseOrder>()
                .WithMany()
                .HasForeignKey(poi => poi.PurchaseOrderId)
                .OnDelete(DeleteBehavior.Restrict);

            purchaseOrderItem.HasOne<Product>()
                .WithMany()
                .HasForeignKey(poi => poi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            purchaseOrderItem.ToTable(table =>
            {
                table.HasCheckConstraint(
                    "CK_PurchaseOrderItems_Quantity_Positive",
                    "\"Quantity\" > 0");
                table.HasCheckConstraint(
                    "CK_PurchaseOrderItems_UnitCost_NonNegative",
                    "\"UnitCost\" >= 0");
            });
        });

        ////////////AuditLog/////////////
        modelBuilder.Entity<AuditLog>(auditLog =>
        {
            auditLog.HasKey(al => al.Id);

            auditLog.Property(al => al.Id)
                .ValueGeneratedOnAdd();

            auditLog.Property(al => al.EntityType)
                .HasMaxLength(100)
                .IsRequired();

            auditLog.Property(al => al.Action)
                .HasMaxLength(100)
                .IsRequired();

            auditLog.Property(al => al.OldValues)
                .HasColumnType("text");

            auditLog.Property(al => al.NewValues)
                .HasColumnType("text");

            auditLog.Property(al => al.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            auditLog.HasOne<User>()
                .WithMany()
                .HasForeignKey(al => al.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
