namespace BookApi.Models;

public class User
{
    public int Id { get; set; }

    public required string Username { get; set; } 

    public required string Email { get; set; } 

    public required string PasswordHash { get; set; } 

    public string Role {get; set; } = Roles.User;

    // Navigation properties for EF Core relationships
    public ICollection<Book> Books { get; set; } = new List<Book>();
    
    public ICollection<Quote> Quotes { get; set; } = new List<Quote>();

}