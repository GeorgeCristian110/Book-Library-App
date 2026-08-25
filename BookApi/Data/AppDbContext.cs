using Microsoft.EntityFrameworkCore;
using BookApi.Models;

namespace BookApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) 
    : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
        .HasIndex(u => u.Email)
        .IsUnique();

        modelBuilder.Entity<User>()
        .HasIndex(u => u.Username)
        .IsUnique();
    }

    public DbSet<User> Users {get; set;}
    public DbSet<Book> Books {get; set;}
    public DbSet<Quote> Quotes {get; set;}

    
}