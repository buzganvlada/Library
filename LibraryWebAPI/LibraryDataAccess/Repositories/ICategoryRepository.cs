using LibraryDataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryDataAccess.Repositories
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetCategoriesAsync();

        Task<Category?> GetCategoryByIdAsync(int id);

        Task<Category> CreateCategoryAsync(Category category);

        Task<Category> UpdateCategoryAsync(Category category);

        Task DeleteCategoryAsync(int id);
    }
}
