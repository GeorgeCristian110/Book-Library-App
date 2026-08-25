using System.Net.Mime;
using System.Security.Claims;
using BookApi.Data;
using BookApi.DTOs.Books;
using BookApi.DTOs.Quotes;
using BookApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace BookApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]

public class QuotesController : ControllerBase
{
    private readonly AppDbContext _context;

    public QuotesController (AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<QuoteResponse>>> GetMyQuotes()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        
        var quotes = await _context.Quotes
        .Include(q => q.Book)
        .Where(q => q.OwnerId == userId)
        .ToListAsync();

        var response = quotes.Select(q => new QuoteResponse
        {
           Id = q.Id,
           Text = q.Text,
           Author = q.Author,
           BookTitle = q.Book != null ?  q.Book.Title : null
        });

        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<QuoteResponse>> CreateQuote(QuoteCreateRequest request)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var quote = new Quote
        {
          Text = request.Text,
          Author = request.Author,
          BookId = request.BookId,
          OwnerId = userId

        };

        _context.Quotes.Add(quote);
        await _context.SaveChangesAsync();

        Book? book = null;

        if(request.BookId != null)
        {
            book = await _context.Books.FindAsync(request.BookId);
        } 
        

        var response = new QuoteResponse
        {
            Id = quote.Id,
            Text = quote.Text,
            Author = quote.Author,
            BookTitle = book != null ? book.Title : null

        };

        return Ok(response);

    }

    [HttpPut("{id}")]
    public async Task<ActionResult<QuoteResponse>> UpdateQuote(int id, QuoteUpdateRequest request)
    {
        var quote = await _context.Quotes
        .Include(q => q.Owner)
        .Include(q => q.Book)
        .FirstOrDefaultAsync(q => q.Id == id);

        if(quote == null)
        {
            return NotFound();
        }

        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var isAdmin = User.IsInRole(Roles.Admin);

        if(quote.OwnerId != userId && !isAdmin)
        {
            return Forbid();
        }

        quote.Text = request.Text;
        quote.Author = request.Author;
        quote.BookId = request.BookId;

        await _context.SaveChangesAsync();

        Book? updatedBook = quote.BookId != null
        ? await _context.Books.FindAsync(quote.BookId)
        : null;

        var response = new QuoteResponse
        {
          Id = quote.Id,
          Text = quote.Text,
          Author = quote.Author,
          BookTitle = updatedBook?.Title
        };

        return Ok(response);
        
    }
}