using ECommerceSystem;
using Microsoft.EntityFrameworkCore;

using var db = new AppDbContext();
db.Database.EnsureDeleted();
db.Database.EnsureCreated();

// ---------- Seed data ----------
var electronics = new Category { Name = "Electronics" };
var books = new Category { Name = "Books" };

var laptop = new Product { Name = "Laptop", Price = 25000m, Category = electronics };
var phone = new Product { Name = "Phone", Price = 12000m, Category = electronics };
var csharpBook = new Product { Name = "C# in Depth", Price = 600m, Category = books };

var ahmed = new Customer { Name = "Ahmed Ali", Email = "ahmed@example.com" };
var sara = new Customer { Name = "Sara Hassan", Email = "sara@example.com" };

var order1 = new Order
{
    OrderDate = new DateTime(2026, 10, 1),
    Customer = ahmed,
    OrderDetails =
    {
        new OrderDetail { Product = laptop, Quantity = 1 },
        new OrderDetail { Product = csharpBook, Quantity = 2 }
    }
};

var order2 = new Order
{
    OrderDate = new DateTime(2026, 10, 5),
    Customer = sara,
    OrderDetails =
    {
        new OrderDetail { Product = phone, Quantity = 1 },
        new OrderDetail { Product = csharpBook, Quantity = 1 }
    }
};

db.Orders.AddRange(order1, order2);
db.SaveChanges();

// ---------- Queries using navigation properties ----------
Console.WriteLine("=== Categories and their Products (One-to-Many) ===");
foreach (var c in db.Categories.Include(c => c.Products))
{
    Console.WriteLine($"{c.Name}:");
    foreach (var p in c.Products)
        Console.WriteLine($"   - {p.Name} ({p.Price:N2} EGP)");
}

Console.WriteLine("\n=== Customers and their Orders (One-to-Many) ===");
foreach (var cu in db.Customers.Include(c => c.Orders))
{
    Console.WriteLine($"{cu.Name} ({cu.Email}) has {cu.Orders.Count} order(s)");
}

Console.WriteLine("\n=== Orders with Products (Many-to-Many through OrderDetail) ===");
var orders = db.Orders
    .Include(o => o.Customer)
    .Include(o => o.OrderDetails)
        .ThenInclude(od => od.Product)
            .ThenInclude(p => p.Category)
    .ToList();

foreach (var o in orders)
{
    Console.WriteLine($"Order #{o.Id} - {o.OrderDate:yyyy-MM-dd} - Customer: {o.Customer.Name}");
    foreach (var od in o.OrderDetails)
    {
        Console.WriteLine($"   {od.Product.Name} [{od.Product.Category.Name}] x{od.Quantity} = {od.Product.Price * od.Quantity:N2} EGP");
    }
    Console.WriteLine($"   Total: {o.OrderDetails.Sum(od => od.Product.Price * od.Quantity):N2} EGP");
}

Console.WriteLine("\n=== Which orders contain 'C# in Depth'? (Product -> Orders) ===");
var book = db.Products
    .Include(p => p.OrderDetails)
        .ThenInclude(od => od.Order)
            .ThenInclude(o => o.Customer)
    .First(p => p.Name == "C# in Depth");

foreach (var od in book.OrderDetails)
    Console.WriteLine($"Order #{od.OrderId} by {od.Order.Customer.Name}, quantity {od.Quantity}");
