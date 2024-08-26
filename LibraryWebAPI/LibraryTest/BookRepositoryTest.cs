using LibraryDataAccess.Data;
using LibraryDataAccess.Models;
using LibraryDataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

public class BookRepositoryTests
{
    private readonly List<Book> _books;

    public BookRepositoryTests()
    {
        _books = new List<Book>
        {
            new Book { BookId = 1, Title = "Test Book 1", Price = 9.99m },
            new Book { BookId = 2, Title = "Test Book 2", Price = 12.99m }
        };
    }

    [Fact]
    public async Task BookRepositoryGetAllBooks_ShouldReturnZeroBooks_IfNone()
    {
        var options = new DbContextOptionsBuilder<LibraryContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new LibraryContext(options);
        IBookRepository repo = new BookRepository(context);

        Assert.False(context.Books.Any());
        var books = await repo.GetBooksAsync();
        Assert.Empty(books);
    }

    [Fact]
    public async Task BookRepositoryGetAllBooks_ShouldReturnBooks_IfAny()
    {
        var options = new DbContextOptionsBuilder<LibraryContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new LibraryContext(options);
        await context.Books.AddRangeAsync(_books);
        await context.SaveChangesAsync();
        IBookRepository repo = new BookRepository(context);

        Assert.True(context.Books.Any());
        var books = await repo.GetBooksAsync();
        Assert.Equal(_books.Count, books.Count);
        Assert.Equal(_books, books);
    }

    [Fact]
    public async Task BookRepositoryGetBookById_ShouldReturnNull_IfNone()
    {
        var options = new DbContextOptionsBuilder<LibraryContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new LibraryContext(options);
        IBookRepository repo = new BookRepository(context);

        Assert.False(context.Books.Any());
        var book = await repo.GetBookByIdAsync(1);
        Assert.Null(book);
    }

    [Fact]
    public async Task BookRepositoryGetBookById_ShouldReturnBook_IfFound()
    {
        var options = new DbContextOptionsBuilder<LibraryContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new LibraryContext(options);
        await context.Books.AddAsync(_books[0]);
        await context.SaveChangesAsync();
        IBookRepository repo = new BookRepository(context);

        Assert.True(context.Books.Any());
        var book = await repo.GetBookByIdAsync(_books[0].BookId);
        Assert.NotNull(book);
        Assert.Equal(_books[0], book);
    }

    [Fact]
    public async Task BookRepositoryCreateBook_ShouldCreateBook_IfNone()
    {
        var options = new DbContextOptionsBuilder<LibraryContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new LibraryContext(options);
        IBookRepository repo = new BookRepository(context);

        Assert.Null(await context.Books.FirstOrDefaultAsync(b => b.BookId == _books[0].BookId));
        var book = await repo.CreateBookAsync(_books[0]);
        Assert.NotNull(book);
        Assert.Equal(_books[0], book);
    }

    [Fact]
    public async Task BookRepositoryDeleteBook_ShouldThrowException_IfNone()
    {
        var options = new DbContextOptionsBuilder<LibraryContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new LibraryContext(options);
        IBookRepository repo = new BookRepository(context);

        Assert.Null(await context.Books.FirstOrDefaultAsync(b => b.BookId == _books[0].BookId));
        await Assert.ThrowsAsync<KeyNotFoundException>(() => repo.DeleteBookAsync(_books[0].BookId));
    }

    [Fact]
    public async Task BookRepositoryDeleteBook_ShouldRemoveBook_IfFound()
    {
        var options = new DbContextOptionsBuilder<LibraryContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new LibraryContext(options);
        await context.Books.AddAsync(_books[0]);
        await context.SaveChangesAsync();
        IBookRepository repo = new BookRepository(context);

        Assert.NotNull(await context.Books.FirstOrDefaultAsync(b => b.BookId == _books[0].BookId));
        await repo.DeleteBookAsync(_books[0].BookId);
        await Assert.ThrowsAsync<KeyNotFoundException>(() => repo.DeleteBookAsync(_books[0].BookId));
    }

    [Fact]
    public async Task BookRepositoryUpdateBook_ShouldThrowException_IfNone()
    {
        var options = new DbContextOptionsBuilder<LibraryContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new LibraryContext(options);
        IBookRepository repo = new BookRepository(context);

        Assert.Null(await context.Books.FirstOrDefaultAsync(b => b.BookId == _books[0].BookId));
        await Assert.ThrowsAsync<KeyNotFoundException>(() => repo.UpdateBookAsync(_books[0]));
    }

    [Fact]
    public async Task BookRepositoryUpdateBook_ShouldUpdateBook_IfFound()
    {
        var options = new DbContextOptionsBuilder<LibraryContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new LibraryContext(options);
        await context.Books.AddAsync(_books[0]);
        await context.SaveChangesAsync();

        var updatedBook = new Book
        {
            BookId = _books[0].BookId,
            Title = "Updated Title",
            Price = 15.99m
        };
        IBookRepository repo = new BookRepository(context);

        Assert.NotNull(await context.Books.FirstOrDefaultAsync(b => b.BookId == _books[0].BookId));
        var bookUpdated = await repo.UpdateBookAsync(updatedBook);
        Assert.Equal(updatedBook.Title, bookUpdated.Title);
        Assert.Equal(updatedBook.Price, bookUpdated.Price);
    }
}
