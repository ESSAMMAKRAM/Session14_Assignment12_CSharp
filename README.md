# Session14_Assignment12_CSharp

Entity Framework Core relationship mapping with navigation properties.

| Project | Entities | Relationships |
|---|---|---|
| `ECommerceSystem` | Category, Product, Customer, Order, OrderDetail | Category 1-N Product, Customer 1-N Order, Order N-N Product (OrderDetail) |
| `LibrarySystem` | Author, Book, Borrower, Loan | Author 1-N Book, Book N-N Borrower (Loan) |
| `HealthCareSystem` | Patient, Doctor, Appointment | Patient N-N Doctor (Appointment) |

Database diagrams are in the `Diagrams` folder.

## Run

```bash
cd ECommerceSystem   # or LibrarySystem / HealthCareSystem
dotnet run
```

Uses SQLite (no server needed). Each project creates its own `.db` file on run.
To use SQL Server instead, change `UseSqlite(...)` in `AppDbContext.cs` to `UseSqlServer(...)`
and replace the package with `Microsoft.EntityFrameworkCore.SqlServer`.
