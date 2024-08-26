﻿using LibraryDataAccess.Data;
using LibraryDataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var authorAdded = await _context.Authors.AddAsync(author);
            await _context.SaveChangesAsync();
            return authorAdded.Entity;
        }

        public async Task DeleteAuthorAsync(int id)
        {
            var authorToDelete = await _context.Authors.FirstOrDefaultAsync(a => a.AuthorId == id);
            if (authorToDelete == null)
            {
                throw new KeyNotFoundException("Author not found.");
            }

            _context.Authors.Remove(authorToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Author>> GetAuthorsAsync()
        {
            return await _context.Authors.ToListAsync();
        }

        public async Task<Author> GetAuthorByIdAsync(int id)
        {
            return await _context.Authors.FirstOrDefaultAsync(a => a.AuthorId == id);
        }

        public async Task<Author> UpdateAuthorAsync(Author author)
        {
            var authorToUpdate = await _context.Authors.FirstOrDefaultAsync(a => a.AuthorId == author.AuthorId);
            if (authorToUpdate == null)
            {
                throw new KeyNotFoundException("Author not found.");
            }

            authorToUpdate.AuthorName = author.AuthorName;

            await _context.SaveChangesAsync();
            return authorToUpdate;
        }
    }
}
