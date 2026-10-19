namespace LibrarySystem;

public class Author
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }

    // One-to-Many: Author -> Books
    public ICollection<Book> Books { get; set; } = new List<Book>();
}

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;

    // Many-to-One: Book -> Author
    public int AuthorId { get; set; }
    public Author Author { get; set; } = null!;

    // Many-to-Many with Borrower (through Loan)
    public ICollection<Loan> Loans { get; set; } = new List<Loan>();
}

public class Borrower
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime MembershipDate { get; set; }

    // Many-to-Many with Book (through Loan)
    public ICollection<Loan> Loans { get; set; } = new List<Loan>();
}

// Join entity: Book <-> Borrower
public class Loan
{
    public int BookId { get; set; }
    public int BorrowerId { get; set; }
    public DateTime LoanDate { get; set; }
    public DateTime? ReturnDate { get; set; }   // null = not returned yet

    public Book Book { get; set; } = null!;
    public Borrower Borrower { get; set; } = null!;
}
