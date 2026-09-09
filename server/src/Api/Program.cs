using Microsoft.EntityFrameworkCore;
using WM.Application.Services;
using WM.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Design;
using WM.Api.ExceptionHandling;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("WarehouseDatabase")
    ?? throw new InvalidOperationException("Connection String 'WarehouseDatabase' was not found.");

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>options.UseNpgsql(connectionString));



// here ill add all the AddScoped
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddScoped<IWarehouseRepository, WarehouseRepository>();
builder.Services.AddScoped<IWarehouseService, WarehouseService>();








builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();


var app = builder.Build();
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    // using var scope = app.Services.CreateScope();

    // var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    // await DatabaseBootstrapper.InitializeAsync(dbContext, connectionString, args);

    /////////////////////////////////////////////////////////////////////////////////////////////



    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint(
            "/openapi/v1.json",
            "Warehouse Management API v1");
    });
}



app.UseAuthorization();

app.MapControllers();

app.Run();