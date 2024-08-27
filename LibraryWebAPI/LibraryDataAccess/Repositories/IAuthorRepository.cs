
using LibraryDataAccess.Models;
using Libray.Core;

namespace LibraryDataAccess.Repositories
{
    public interface IAuthorRepository
    {
        Task<PaginatedList<Author>> GetAuthorsAsync(int page = 1, int nr = 10);

        Task<Author?> GetAuthorByIdAsync(int id);

        Task<Author> CreateAuthorAsync(Author author);

        Task<Author> UpdateAuthorAsync(Author author);

        Task DeleteAuthorAsync(int id);
    }
}
