using BookApi.Models;

namespace BookApi.DTOs.Books;

public record BookResponse
{
    public required int Id {get; set;}
    public required string Title {get; set;}
    public required string Author {get; set;}
    public required DateOnly PublishedDate {get; set;}
    public required Genre Genre {get; set;}
    public string? Description {get; set;}
    public required string OwnerUsername {get ; set;}
}