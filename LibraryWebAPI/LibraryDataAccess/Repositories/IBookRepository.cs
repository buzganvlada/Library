
using LibraryDataAccess.Models;
using Libray.Core;

namespace LibraryDataAccess.Repositories
{
    public interface IBookRepository
    {
        Task<PaginatedList<Book>> GetBooksAsync(int page = 1, int nr = 10);

        Task<Book?> GetBookByIdAsync(int id);

        Task<Book> CreateBookAsync(Book book);

        Task<Book> UpdateBookAsync(Book book);

        Task DeleteBookAsync(int id);
    }
}
