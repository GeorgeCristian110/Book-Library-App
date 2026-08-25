using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace BookApi.Models;

public class Book
{
    public int Id { get; set; }

    public required string Title { get; set; } 

    public required string Author { get; set; } 

    public DateOnly PublishedDate { get; set; }

    public required Genre Genre { get; set; } 

    [MaxLength(200)]
    public string? Description {get; set;}

    public int OwnerId {get; set;}

    public User? Owner {get; set;}

    // A book can have many quotes attached to it
    public ICollection<Quote> Quotes { get; set; } = new List<Quote>();

  
}