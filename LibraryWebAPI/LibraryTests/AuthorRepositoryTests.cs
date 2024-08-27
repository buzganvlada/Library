using LibraryDataAccess.Data;
using LibraryDataAccess.Models;
using LibraryDataAccess.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Library.Tests
{
    public class AuthorRepositoryTests
    {
        private List<Author> _authors = new List<Author>
        {
            new Author {AuthorId = 1, AuthorName = "FirstAuthor"},
            new Author {AuthorId = 2, AuthorName = "SecondAuthor"},
            new Author {AuthorId = 3, AuthorName = "ThirdAuthor"},
            new Author {AuthorId = 4, AuthorName = "ForthAuthor"},
        };


        [Fact]
        public async Task AuthorRepositoryGetAllAuthors_ShouldReturnZeroAuthors_IfNone()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new LibraryContext(options);
            IAuthorRepository repo = new AuthorRepository(context);

            Assert.True(!context.Authors.Any());
            var authors = await repo.GetAuthorsAsync();
            Assert.True(authors.Items.Count == 0);
        }

        [Fact]
        public async Task AuthorRepositoryGetAllAuthors_ShouldReturnAuthors_IfAny()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new LibraryContext(options);
            await context.Authors.AddRangeAsync(_authors);
            await context.SaveChangesAsync();
            IAuthorRepository repo = new AuthorRepository(context);

            Assert.True(context.Authors.Any());
            var authors = await repo.GetAuthorsAsync();
            Assert.True(authors.Items.Any());
            Assert.True(authors.Items.Count == _authors.Count);
            Assert.Equal(_authors, authors.Items);
        }

        [Fact]
        public async Task AuthorRepositoryGetAuthorById_ShouldReturnNull_IfNone()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new LibraryContext(options);
            IAuthorRepository repo = new AuthorRepository(context);

            Assert.True(!context.Authors.Any());
            var author = await repo.GetAuthorByIdAsync(1);
            Assert.Null(author);
        }

        [Fact]
        public async Task AuthorRepositoryGetAuthorById_ShouldReturnAuthor_IfFound()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new LibraryContext(options);
            await context.Authors.AddAsync(_authors.ElementAt(0));
            await context.SaveChangesAsync();
            IAuthorRepository repo = new AuthorRepository(context);

            Assert.True(context.Authors.Any());
            var author = await repo.GetAuthorByIdAsync(_authors.ElementAt(0).AuthorId);
            Assert.NotNull(author);
            Assert.Equal(_authors.ElementAt(0), author);
        }

        [Fact]
        public async Task AuthorRepositoryCreateAuthor_ShouldCreateAuthor_IfNone()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new LibraryContext(options);
            IAuthorRepository repo = new AuthorRepository(context);


            Assert.Null(await context.Authors.FirstOrDefaultAsync(c => c.AuthorId == _authors.ElementAt(0).AuthorId));
            var author = await repo.CreateAuthorAsync(_authors.ElementAt(0));
            Assert.NotNull(author);
            Assert.Equal(_authors.ElementAt(0), author);
        }

        [Fact]
        public async Task AuthorRepositoryDeleteAuthor_ShouldThrowException_IfNone()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new LibraryContext(options);
            IAuthorRepository repo = new AuthorRepository(context);


            Assert.Null(await context.Authors.FirstOrDefaultAsync(c => c.AuthorId == _authors.ElementAt(0).AuthorId));

            await Assert.ThrowsAsync<KeyNotFoundException>(() => repo.DeleteAuthorAsync(_authors.ElementAt(0).AuthorId));
        }

        [Fact]
        public async Task AuthorRepositoryDeleteAuthor_ShouldRemoveAuthor_IfFound()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new LibraryContext(options);
            await context.Authors.AddAsync(_authors.ElementAt(0));
            await context.SaveChangesAsync();
            IAuthorRepository repo = new AuthorRepository(context);

            Assert.NotNull(await context.Authors.FirstOrDefaultAsync(c => c.AuthorId == _authors.ElementAt(0).AuthorId));
            await repo.DeleteAuthorAsync(_authors.ElementAt(0).AuthorId);
            await Assert.ThrowsAsync<KeyNotFoundException>(() => repo.DeleteAuthorAsync(_authors.ElementAt(0).AuthorId));
        }


        [Fact]
        public async Task AuthorRepositoryUpdateAuthor_ShouldThrowException_IfNone()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new LibraryContext(options);
            IAuthorRepository repo = new AuthorRepository(context);


            Assert.Null(await context.Authors.FirstOrDefaultAsync(c => c.AuthorId == _authors.ElementAt(0).AuthorId));

            await Assert.ThrowsAsync<KeyNotFoundException>(() => repo.UpdateAuthorAsync(_authors.ElementAt(0)));
        }

        [Fact]
        public async Task AuthorRepositoryUpdateAuthor_ShouldUpdateAuthor_IfFound()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new LibraryContext(options);
            await context.Authors.AddAsync(_authors.ElementAt(0));
            await context.SaveChangesAsync();

            var authorToUpdate = new Author
            {
                AuthorName = "UpdatedName",
                AuthorId = _authors.ElementAt(0).AuthorId
            };
            IAuthorRepository repo = new AuthorRepository(context);

            Assert.NotNull(await context.Authors.FirstOrDefaultAsync(c => c.AuthorId == _authors.ElementAt(0).AuthorId));

            var authorUpdated = await repo.UpdateAuthorAsync(authorToUpdate);
            Assert.NotSame(authorToUpdate, authorUpdated);
            Assert.Equal(authorToUpdate.AuthorName, authorUpdated.AuthorName);
        }
    }
}