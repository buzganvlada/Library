using LibraryDataAccess.Data;
using LibraryDataAccess.Models;
using LibraryDataAccess.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Library.Tests
{
    public class BookRepositoryTests
    {
        private List<Book> _books = new List<Book>
        {
            new Book {BookId = 1, Title = "FirstBook", Price = 1.00m},
            new Book {BookId = 2, Title = "SecondBook", Price = 1.01m},
            new Book {BookId = 3, Title = "ThirdBook", Price = 1.02m},
            new Book {BookId = 4, Title = "ForthBook", Price = 1.03m},
        };


        [Fact]
        public async Task BookRepositoryGetAllBooks_ShouldReturnZeroBooks_IfNone()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new LibraryContext(options);
            IBookRepository repo = new BookRepository(context);

            Assert.True(!context.Books.Any());
            var books = await repo.GetBooksAsync();
            Assert.True(books.Items.Count == 0);
        }

        [Fact]
        public async Task BookRepositoryGetAllBooks_ShouldReturnBooks_IfAny()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new LibraryContext(options);
            await context.Books.AddRangeAsync(_books);
            await context.SaveChangesAsync();
            IBookRepository repo = new BookRepository(context);

            Assert.True(context.Books.Any());
            var books = await repo.GetBooksAsync();
            Assert.True(books.Items.Any());
            Assert.True(books.Items.Count == _books.Count);
            Assert.Equal(_books, books.Items);
        }

        [Fact]
        public async Task BookRepositoryGetBookById_ShouldReturnNull_IfNone()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new LibraryContext(options);
            IBookRepository repo = new BookRepository(context);

            Assert.True(!context.Books.Any());
            var book = await repo.GetBookByIdAsync(1);
            Assert.Null(book);
        }

        [Fact]
        public async Task BookRepositoryGetBookById_ShouldReturnBook_IfFound()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new LibraryContext(options);
            await context.Books.AddAsync(_books.ElementAt(0));
            await context.SaveChangesAsync();
            IBookRepository repo = new BookRepository(context);

            Assert.True(context.Books.Any());
            var book = await repo.GetBookByIdAsync(_books.ElementAt(0).BookId);
            Assert.NotNull(book);
            Assert.Equal(_books.ElementAt(0), book);
        }

        [Fact]
        public async Task BookRepositoryCreateBook_ShouldCreateBook_IfNone()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new LibraryContext(options);
            IBookRepository repo = new BookRepository(context);


            Assert.Null(await context.Books.FirstOrDefaultAsync(c => c.BookId == _books.ElementAt(0).BookId));
            var book = await repo.CreateBookAsync(_books.ElementAt(0));
            Assert.NotNull(book);
            Assert.Equal(_books.ElementAt(0), book);
        }

        [Fact]
        public async Task BookRepositoryDeleteBook_ShouldThrowException_IfNone()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new LibraryContext(options);
            IBookRepository repo = new BookRepository(context);


            Assert.Null(await context.Books.FirstOrDefaultAsync(c => c.BookId == _books.ElementAt(0).BookId));

            await Assert.ThrowsAsync<KeyNotFoundException>(() => repo.DeleteBookAsync(_books.ElementAt(0).BookId));
        }

        [Fact]
        public async Task BookRepositoryDeleteBook_ShouldRemoveBook_IfFound()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new LibraryContext(options);
            await context.Books.AddAsync(_books.ElementAt(0));
            await context.SaveChangesAsync();
            IBookRepository repo = new BookRepository(context);

            Assert.NotNull(await context.Books.FirstOrDefaultAsync(c => c.BookId == _books.ElementAt(0).BookId));
            await repo.DeleteBookAsync(_books.ElementAt(0).BookId);
            await Assert.ThrowsAsync<KeyNotFoundException>(() => repo.DeleteBookAsync(_books.ElementAt(0).BookId));
        }


        [Fact]
        public async Task BookRepositoryUpdateBook_ShouldThrowException_IfNone()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new LibraryContext(options);
            IBookRepository repo = new BookRepository(context);


            Assert.Null(await context.Books.FirstOrDefaultAsync(c => c.BookId == _books.ElementAt(0).BookId));

            await Assert.ThrowsAsync<KeyNotFoundException>(() => repo.UpdateBookAsync(_books.ElementAt(0)));
        }

        [Fact]
        public async Task BookRepositoryUpdateBook_ShouldUpdateBook_IfFound()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new LibraryContext(options);
            await context.Books.AddAsync(_books.ElementAt(0));
            await context.SaveChangesAsync();

            var bookToUpdate = new Book
            {
                Title = "UpdatedName",
                Price = 2.00m,
                BookId = _books.ElementAt(0).BookId
            };
            IBookRepository repo = new BookRepository(context);

            Assert.NotNull(await context.Books.FirstOrDefaultAsync(c => c.BookId == _books.ElementAt(0).BookId));

            var bookUpdated = await repo.UpdateBookAsync(bookToUpdate);
            Assert.NotSame(bookToUpdate, bookUpdated);
            Assert.Equal(bookToUpdate.Title, bookUpdated.Title);
            Assert.Equal(bookToUpdate.Price, bookUpdated.Price);
        }
    }
}
