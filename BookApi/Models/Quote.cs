namespace BookApi.Models;

public class Quote
{
    public int Id {get; set;}

    public required string Text {get; set;}

    public string? Author {get; set;}

    public int OwnerId {get; set;}
    public User? Owner {get; set;}

    public int? BookId {get; set;}
    public Book? Book {get; set;}
}