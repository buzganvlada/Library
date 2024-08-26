using LibraryDataAccess.Data;
using LibraryDataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryDataAccess.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly LibraryContext _context;

        public BookRepository(LibraryContext context)
        {
            _context = context;
        }

        public async Task<Book> CreateBookAsync(Book book)
        {
            var bookAdded = await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
            return bookAdded.Entity;
        }

        public async Task DeleteBookAsync(int id)
        {
            var bookToDelete = await _context.Books.FirstOrDefaultAsync(b => b.BookId == id);
            if (bookToDelete == null)
            {
                throw new KeyNotFoundException("Book not found.");
            }

            _context.Books.Remove(bookToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Book>> GetBooksAsync()
        {
            return await _context.Books.ToListAsync();
        }

        public async Task<Book> GetBookByIdAsync(int id)
        {
            return await _context.Books.FirstOrDefaultAsync(b => b.BookId == id);
        }

        public async Task<Book> UpdateBookAsync(Book book)
        {
            var bookToUpdate = await _context.Books.FirstOrDefaultAsync(b => b.BookId == book.BookId);
            if (bookToUpdate == null)
            {
                throw new KeyNotFoundException("Book not found.");
            }

            bookToUpdate.Title = book.Title;
            bookToUpdate.Price = book.Price;
            bookToUpdate.CategoryId = book.CategoryId;
            bookToUpdate.AuthorId = book.AuthorId;

            await _context.SaveChangesAsync();
            return bookToUpdate;
        }
    }
}
