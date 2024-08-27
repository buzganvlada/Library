
using LibraryDataAccess.Models;
using Libray.Core;

namespace LibraryDataAccess.Repositories
{
    public interface ICategoryRepository
    {
        Task<PaginatedList<Category>> GetCategoriesAsync(int page = 1, int nr = 10);

        Task<Category?> GetCategoryByIdAsync(int id);

        Task<Category> CreateCategoryAsync(Category category);

        Task<Category> UpdateCategoryAsync(Category category);

        Task DeleteCategoryAsync(int id);
    }
}
