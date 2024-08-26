using LibraryDataAccess.Data;
using LibraryDataAccess.Models;
using LibraryDataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

public class AuthorRepositoryTests
{
    private readonly List<Author> _authors;

    public AuthorRepositoryTests()
    {
        _authors = new List<Author>
        {
            new Author { AuthorId = 1, AuthorName = "Test Author 1" },
            new Author { AuthorId = 2, AuthorName = "Test Author 2" }
        };
    }

    [Fact]
    public async Task AuthorRepositoryGetAllAuthors_ShouldReturnZeroAuthors_IfNone()
    {
        var options = new DbContextOptionsBuilder<LibraryContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new LibraryContext(options);
        IAuthorRepository repo = new AuthorRepository(context);

        Assert.False(context.Authors.Any());
        var authors = await repo.GetAuthorsAsync();
        Assert.Empty(authors);
    }

    [Fact]
    public async Task AuthorRepositoryGetAllAuthors_ShouldReturnAuthors_IfAny()
    {
        var options = new DbContextOptionsBuilder<LibraryContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new LibraryContext(options);
        await context.Authors.AddRangeAsync(_authors);
        await context.SaveChangesAsync();
        IAuthorRepository repo = new AuthorRepository(context);

        Assert.True(context.Authors.Any());
        var authors = await repo.GetAuthorsAsync();
        Assert.Equal(_authors.Count, authors.Count);
        Assert.Equal(_authors, authors);
    }

    [Fact]
    public async Task AuthorRepositoryGetAuthorById_ShouldReturnNull_IfNone()
    {
        var options = new DbContextOptionsBuilder<LibraryContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new LibraryContext(options);
        IAuthorRepository repo = new AuthorRepository(context);

        Assert.False(context.Authors.Any());
        var author = await repo.GetAuthorByIdAsync(1);
        Assert.Null(author);
    }

    [Fact]
    public async Task AuthorRepositoryGetAuthorById_ShouldReturnAuthor_IfFound()
    {
        var options = new DbContextOptionsBuilder<LibraryContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new LibraryContext(options);
        await context.Authors.AddAsync(_authors[0]);
        await context.SaveChangesAsync();
        IAuthorRepository repo = new AuthorRepository(context);

        Assert.True(context.Authors.Any());
        var author = await repo.GetAuthorByIdAsync(_authors[0].AuthorId);
        Assert.NotNull(author);
        Assert.Equal(_authors[0], author);
    }

    [Fact]
    public async Task AuthorRepositoryCreateAuthor_ShouldCreateAuthor_IfNone()
    {
        var options = new DbContextOptionsBuilder<LibraryContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new LibraryContext(options);
        IAuthorRepository repo = new AuthorRepository(context);

        Assert.Null(await context.Authors.FirstOrDefaultAsync(a => a.AuthorId == _authors[0].AuthorId));
        var author = await repo.CreateAuthorAsync(_authors[0]);
        Assert.NotNull(author);
        Assert.Equal(_authors[0], author);
    }

    [Fact]
    public async Task AuthorRepositoryDeleteAuthor_ShouldThrowException_IfNone()
    {
        var options = new DbContextOptionsBuilder<LibraryContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new LibraryContext(options);
        IAuthorRepository repo = new AuthorRepository(context);

        Assert.Null(await context.Authors.FirstOrDefaultAsync(a => a.AuthorId == _authors[0].AuthorId));
        await Assert.ThrowsAsync<KeyNotFoundException>(() => repo.DeleteAuthorAsync(_authors[0].AuthorId));
    }

    [Fact]
    public async Task AuthorRepositoryDeleteAuthor_ShouldRemoveAuthor_IfFound()
    {
        var options = new DbContextOptionsBuilder<LibraryContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new LibraryContext(options);
        await context.Authors.AddAsync(_authors[0]);
        await context.SaveChangesAsync();
        IAuthorRepository repo = new AuthorRepository(context);

        Assert.NotNull(await context.Authors.FirstOrDefaultAsync(a => a.AuthorId == _authors[0].AuthorId));
        await repo.DeleteAuthorAsync(_authors[0].AuthorId);
        await Assert.ThrowsAsync<KeyNotFoundException>(() => repo.DeleteAuthorAsync(_authors[0].AuthorId));
    }

    [Fact]
    public async Task AuthorRepositoryUpdateAuthor_ShouldThrowException_IfNone()
    {
        var options = new DbContextOptionsBuilder<LibraryContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new LibraryContext(options);
        IAuthorRepository repo = new AuthorRepository(context);

        Assert.Null(await context.Authors.FirstOrDefaultAsync(a => a.AuthorId == _authors[0].AuthorId));
        await Assert.ThrowsAsync<KeyNotFoundException>(() => repo.UpdateAuthorAsync(_authors[0]));
    }

    [Fact]
    public async Task AuthorRepositoryUpdateAuthor_ShouldUpdateAuthor_IfFound()
    {
        var options = new DbContextOptionsBuilder<LibraryContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new LibraryContext(options);
        await context.Authors.AddAsync(_authors[0]);
        await context.SaveChangesAsync();

        var updatedAuthor = new Author
        {
            AuthorId = _authors[0].AuthorId,
            AuthorName = "Updated Author Name"
        };
        IAuthorRepository repo = new AuthorRepository(context);

        Assert.NotNull(await context.Authors.FirstOrDefaultAsync(a => a.AuthorId == _authors[0].AuthorId));
        var authorUpdated = await repo.UpdateAuthorAsync(updatedAuthor);
        Assert.Equal(updatedAuthor.AuthorName, authorUpdated.AuthorName);
    }
}

