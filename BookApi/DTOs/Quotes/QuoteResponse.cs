
namespace BookApi.DTOs.Quotes;

public record QuoteResponse
{
    public required int Id {get ; set;}
    public required string Text {get ; set;}
    public string? Author {get ; set;}
    public string? BookTitle {get; set;}
}