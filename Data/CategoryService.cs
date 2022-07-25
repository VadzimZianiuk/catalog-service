using CatalogService.DBContext;
using CatalogService.Interfaces;
using CatalogService.Models;
using System.Linq.Expressions;

namespace CatalogService.Data
{
    public class CategoryService : ICategoryService
    {
        private readonly ApiContext _dbContext;
        public CategoryService(ApiContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Category> GetCategories() => _dbContext.Categories;

        public IEnumerable<Category> GetCategories(Expression<Func<Category, bool>> predicate) =>
            _dbContext.Categories.Where(predicate);

        public async Task<Category> AddCategoryAsync(Category category)
        {
            if (category is null)
                throw new ArgumentNullException(nameof(category));

            category = _dbContext.Categories.Add(category).Entity;
            await _dbContext.SaveChangesAsync();

            return category;
        }

        public async Task UpdateCategoryAsync(int id, Category category)
        {
            if (category is null)
                throw new ArgumentNullException(nameof(category));

            var data = _dbContext.Categories.FirstOrDefault(x => x.Id == id)
                ?? throw new ArgumentException($"There is no category with id #{id}", nameof(id));

            data.Name = category.Name;

            _dbContext.Categories.Update(data);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = _dbContext.Categories.FirstOrDefault(x => x.Id == id)
                ?? throw new ArgumentException($"There is no category with id #{id}", nameof(id));

            _dbContext.Categories.Remove(category);

            await _dbContext.SaveChangesAsync();
        }
    }
}
