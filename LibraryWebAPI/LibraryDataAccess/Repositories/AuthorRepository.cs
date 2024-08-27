using LibraryDataAccess.Data;
using LibraryDataAccess.Models;
using Libray.Core;
using Microsoft.EntityFrameworkCore;

namespace LibraryDataAccess.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly LibraryContext _context;

        public AuthorRepository(LibraryContext context)
        {
            _context = context;
        }
        public async Task<Author> CreateAuthorAsync(Author author)
        {
            var createdAuthor = await _context.AddAsync(author);
            await _context.SaveChangesAsync();
            return createdAuthor.Entity;
        }

        public async Task DeleteAuthorAsync(int id)
        {

            var authorToDelete = await _context.Authors.FirstOrDefaultAsync(c => c.AuthorId == id);
            if (authorToDelete == null)
            {
                throw new KeyNotFoundException();
            }

            _context.Authors.Remove(authorToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task<Author?> GetAuthorByIdAsync(int id)
        {
            return await _context.Authors.FirstOrDefaultAsync(c => c.AuthorId == id);
        }

        public async Task<PaginatedList<Author>> GetAuthorsAsync(int page, int nr)
        {
            var count = _context.Authors.Count();
            var totalPages = (int)Math.Ceiling(count / (double)nr);
            var authors = await _context.Authors.Skip((page - 1) * nr).Take(nr).ToListAsync();

            return new PaginatedList<Author>(authors, page, totalPages);
        }

        public async Task<Author> UpdateAuthorAsync(Author author)
        {
            var authorToUpdate = await _context.Authors.FirstOrDefaultAsync(c => c.AuthorId == author.AuthorId);
            if (authorToUpdate == null)
            {
                throw new KeyNotFoundException();
            }

            authorToUpdate.AuthorName = author.AuthorName;
            var authorUpdated = _context.Update(authorToUpdate);
            await _context.SaveChangesAsync();
            return authorUpdated.Entity;
        }
    }
}