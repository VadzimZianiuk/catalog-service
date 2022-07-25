using CatalogService.Models;
using System.Linq.Expressions;

namespace CatalogService.Interfaces
{
    public interface ICategoryService
    {
        IEnumerable<Category> GetCategories();
        IEnumerable<Category> GetCategories(Expression<Func<Category, bool>> predicate);
        Task<Category> AddCategoryAsync(Category data);
        Task UpdateCategoryAsync(int id, Category data);
        Task DeleteCategoryAsync(int id);
    }
}