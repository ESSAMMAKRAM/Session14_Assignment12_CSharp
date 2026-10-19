namespace ECommerceSystem;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    // One-to-Many: Category -> Products
    public ICollection<Product> Products { get; set; } = new List<Product>();
}

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }

    // Many-to-One: Product -> Category
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    // Many-to-Many with Order (through OrderDetail)
    public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}

public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    // One-to-Many: Customer -> Orders
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}

public class Order
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }

    // Many-to-One: Order -> Customer
    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;

    // Many-to-Many with Product (through OrderDetail)
    public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}

// Join entity: Order <-> Product
public class OrderDetail
{
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }

    public Order Order { get; set; } = null!;
    public Product Product { get; set; } = null!;
}
