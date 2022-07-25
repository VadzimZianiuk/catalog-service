using CatalogService.DBContext;
using CatalogService.Interfaces;
using CatalogService.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CatalogService.Data
{
    public class ItemService : IItemService
    {
        private readonly ApiContext _dbContext;
        public ItemService(ApiContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Item> GetItems() => _dbContext.Items.Include(x => x.Category);

        public IEnumerable<Item> GetItems(Expression<Func<Item, bool>> predicate) =>
            _dbContext.Items.Include(x => x.Category).Where(predicate);

        public async Task<Item> AddItemAsync(Item data)
        {
            data = _dbContext.Items.Add(data).Entity;
            await _dbContext.SaveChangesAsync();

            return data;
        }

        public async Task UpdateItemAsync(int id, Item data)
        {
            if (data is null)
                throw new ArgumentNullException(nameof(data));

            var item = _dbContext.Items.FirstOrDefault(x => x.Id == id)
                ?? throw new ArgumentException($"There is no item with id #{id}", nameof(id));

            item.Name = data.Name;
            item.Category = data.Category;

            _dbContext.Items.Update(item);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteItemAsync(int id)
        {
            var item = _dbContext.Items.FirstOrDefault(x => x.Id == id)
                ?? throw new ArgumentException($"There is no item with id #{id}", nameof(id));

            _dbContext.Items.Remove(item);
            await _dbContext.SaveChangesAsync();
        }
    }
}
