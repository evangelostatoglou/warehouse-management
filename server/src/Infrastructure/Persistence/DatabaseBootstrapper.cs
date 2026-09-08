using Microsoft.EntityFrameworkCore;
using Npgsql;
using WM.Domain.Entities;

namespace WM.Infrastructure.Persistence;

public static class DatabaseBootstrapper
{
    public static async Task InitializeAsync(
        AppDbContext dbContext,
        string connectionString,
        string[] args,
        CancellationToken cancellationToken = default)
    {
        await EnsureDatabaseExistsAsync(
            connectionString,
            cancellationToken);

        await dbContext.Database.MigrateAsync(cancellationToken);

        var mode = args.FirstOrDefault()?.ToLowerInvariant();

        if (mode == "demo")
        {
            await SeedDemoDataAsync(dbContext, cancellationToken);
        }
        else if (mode is null or "resume")
        {
            // Keep existing database data unchanged.
        }
        else
        {
            throw new ArgumentException(
                "Use either 'demo' or 'resume'.");
        }
    }

    private static async Task EnsureDatabaseExistsAsync(string connectionString, CancellationToken cancellationToken)
    {
        var targetBuilder =
            new NpgsqlConnectionStringBuilder(connectionString);

        var databaseName = targetBuilder.Database;

        if (string.IsNullOrWhiteSpace(databaseName))
        {
            throw new InvalidOperationException(
                "The connection string must include a database name.");
        }

        targetBuilder.Database = "postgres";

        await using var connection =
            new NpgsqlConnection(targetBuilder.ConnectionString);

        await connection.OpenAsync(cancellationToken);

        await using var checkCommand = new NpgsqlCommand(
            "SELECT 1 FROM pg_database WHERE datname = @databaseName;",
            connection);

        checkCommand.Parameters.AddWithValue(
            "databaseName",
            databaseName);

        var databaseExists =
            await checkCommand.ExecuteScalarAsync(cancellationToken)
            is not null;

        if (databaseExists)
        {
            return;
        }

        var safeDatabaseName =
            $"\"{databaseName.Replace("\"", "\"\"")}\"";

        await using var createCommand = new NpgsqlCommand(
            $"CREATE DATABASE {safeDatabaseName} TEMPLATE template0;",
            connection);

        await createCommand.ExecuteNonQueryAsync(cancellationToken);
    }

    private static async Task SeedDemoDataAsync( AppDbContext dbContext, CancellationToken cancellationToken)
    {
        var hasProducts = await dbContext.Products
            .AnyAsync(cancellationToken);

        if (hasProducts)
        {
            return;
        }

        var products = new List<Product>
        {
            new(
                "MOUSE-001",
                "Wireless Mouse",
                "Bluetooth wireless mouse",
                29.99m),

            new(
                "KEYBOARD-001",
                "Mechanical Keyboard",
                "Compact mechanical keyboard",
                89.99m),

            new(
                "MONITOR-001",
                "27-inch Monitor",
                "27-inch IPS monitor",
                249.99m)
        };

        dbContext.Products.AddRange(products);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}