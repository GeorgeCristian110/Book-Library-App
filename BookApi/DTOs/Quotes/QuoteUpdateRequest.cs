
namespace BookApi.DTOs.Quotes;

public record QuoteUpdateRequest
{
   
    public required string Text {get ; set;}
    public string? Author {get ; set;}
    public int? BookId {get; set;}
}