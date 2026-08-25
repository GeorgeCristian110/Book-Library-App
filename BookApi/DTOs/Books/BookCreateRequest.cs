using BookApi.Models;
using Microsoft.AspNetCore.SignalR;

namespace BookApi.DTOs.Books;

public record BookCreateRequest
{
    public required string Title {get; set;}
    public required string Author {get; set;}
    public required DateOnly PublishedDate {get; set;}
    public required Genre Genre {get; set;}
    public string? Description {get; set;}
}