using LibrarySystem;
using Microsoft.EntityFrameworkCore;

using var db = new AppDbContext();
db.Database.EnsureDeleted();
db.Database.EnsureCreated();

// ---------- Seed data ----------
var orwell = new Author { Name = "George Orwell", BirthDate = new DateTime(1903, 6, 25) };
var mahfouz = new Author { Name = "Naguib Mahfouz", BirthDate = new DateTime(1911, 12, 11) };

var book1 = new Book { Title = "1984", ISBN = "978-0451524935", Author = orwell };
var book2 = new Book { Title = "Animal Farm", ISBN = "978-0451526342", Author = orwell };
var book3 = new Book { Title = "Midaq Alley", ISBN = "978-0385264761", Author = mahfouz };

var mona = new Borrower { Name = "Mona Khaled", MembershipDate = new DateTime(2025, 1, 10) };
var omar = new Borrower { Name = "Omar Youssef", MembershipDate = new DateTime(2025, 6, 3) };

db.Loans.AddRange(
    new Loan { Book = book1, Borrower = mona, LoanDate = new DateTime(2026, 9, 1), ReturnDate = new DateTime(2026, 9, 15) },
    new Loan { Book = book2, Borrower = mona, LoanDate = new DateTime(2026, 10, 2) },
    new Loan { Book = book1, Borrower = omar, LoanDate = new DateTime(2026, 10, 10) },
    new Loan { Book = book3, Borrower = omar, LoanDate = new DateTime(2026, 10, 12) }
);
db.SaveChanges();

// ---------- Queries using navigation properties ----------
Console.WriteLine("=== Authors and their Books (One-to-Many) ===");
foreach (var a in db.Authors.Include(a => a.Books))
{
    Console.WriteLine($"{a.Name} (born {a.BirthDate:yyyy-MM-dd})");
    foreach (var b in a.Books)
        Console.WriteLine($"   - {b.Title} [ISBN {b.ISBN}]");
}

Console.WriteLine("\n=== Books and who borrowed them (Many-to-Many through Loan) ===");
var books = db.Books
    .Include(b => b.Author)
    .Include(b => b.Loans)
        .ThenInclude(l => l.Borrower)
    .ToList();

foreach (var b in books)
{
    Console.WriteLine($"{b.Title} by {b.Author.Name}");
    foreach (var l in b.Loans)
    {
        var status = l.ReturnDate is null ? "NOT returned" : $"returned {l.ReturnDate:yyyy-MM-dd}";
        Console.WriteLine($"   borrowed by {l.Borrower.Name} on {l.LoanDate:yyyy-MM-dd} ({status})");
    }
}

Console.WriteLine("\n=== Borrowers and their Books (Borrower -> Books) ===");
var borrowers = db.Borrowers
    .Include(br => br.Loans)
        .ThenInclude(l => l.Book)
    .ToList();

foreach (var br in borrowers)
{
    Console.WriteLine($"{br.Name} (member since {br.MembershipDate:yyyy-MM-dd})");
    foreach (var l in br.Loans)
        Console.WriteLine($"   - {l.Book.Title}");
}

Console.WriteLine("\n=== Books currently on loan (ReturnDate is null) ===");
var onLoan = db.Loans
    .Where(l => l.ReturnDate == null)
    .Include(l => l.Book)
    .Include(l => l.Borrower)
    .ToList();

foreach (var l in onLoan)
    Console.WriteLine($"{l.Book.Title} -> {l.Borrower.Name}");
