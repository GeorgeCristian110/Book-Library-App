using System.Net.Mime;
using System.Security.Claims;
using BookApi.Data;
using BookApi.DTOs.Books;
using BookApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]

public class BooksController : ControllerBase
{
    private readonly AppDbContext _context;

    public BooksController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookResponse>>> GetAllBooks()
    {
        var books = await _context.Books
        .Include(b => b.Owner)
        .ToListAsync();

        var response = books.Select(b => new BookResponse
        {
            Id = b.Id,
            Title = b.Title,
            Author = b.Author,
            PublishedDate = b.PublishedDate,
            Genre = b.Genre,
            Description = b.Description,
            OwnerUsername = b.Owner!.Username
        });

        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<BookResponse>> CreateBook(BookCreateRequest request)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userId = int.Parse(userIdClaim!);

        var book = new Book
        {
            Title = request.Title,
            Author = request.Author,
            PublishedDate = request.PublishedDate,
            Genre = request.Genre,
            Description = request.Description,
            OwnerId = userId
        };

        _context.Books.Add(book);
        await _context.SaveChangesAsync();

        var owner = await _context.Users.FindAsync(userId);

        var response = new BookResponse
        {
          Id = book.Id,
          Title = book.Title,
          Author = book.Author,
          PublishedDate = book.PublishedDate,
          Genre = book.Genre,
          Description = book.Description,
          OwnerUsername = owner!.Username
        };

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task <ActionResult<BookResponse>> GetBookById(int id)
    {
        var book = await _context.Books
        .Include(b => b.Owner)
        .FirstOrDefaultAsync(b => b.Id == id);

        if (book == null)
        {
            return NotFound();
        }

        var response = new BookResponse
        {
          Id = book.Id,
          Title = book.Title,
          Author = book.Author,
          PublishedDate = book.PublishedDate,
          Genre = book.Genre,
          Description = book.Description,
          OwnerUsername = book.Owner!.Username  
        };

        return Ok(response);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<BookResponse>> UpdateBook (int id, BookUpdateRequest request)
    {
        var book = await _context.Books
        .Include(b => b.Owner)
        .FirstOrDefaultAsync(b =>  b.Id == id);

        if(book == null)
        {
            return NotFound();
        }

        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var isAdmin = User.IsInRole(Roles.Admin);

        if(book.OwnerId != userId && !isAdmin)
        {
            return Forbid();
        }

        book.Title = request.Title;
        book.Author = request.Author;
        book.PublishedDate = request.PublishedDate;
        book.Genre = request.Genre;
        book.Description = request.Description;

        await _context.SaveChangesAsync();

        var response = new BookResponse
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            PublishedDate = book.PublishedDate,
            Genre = book.Genre,
            Description = book.Description,
            OwnerUsername = book.Owner!.Username
        };

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBook (int id)
    {
         var book = await _context.Books
        .FirstOrDefaultAsync(b =>  b.Id == id);

        if(book == null)
        {
            return NotFound();
        }

        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var isAdmin = User.IsInRole(Roles.Admin);

        if (book.OwnerId != userId && !isAdmin)
        {
            return Forbid();
        }

        _context.Books.Remove(book);
        await _context.SaveChangesAsync();

        return NoContent();

    }
}