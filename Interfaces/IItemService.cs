using CatalogService.Models;
using System.Linq.Expressions;

namespace CatalogService.Interfaces
{
    public interface IItemService
    {
        IEnumerable<Item> GetItems();
        IEnumerable<Item> GetItems(Expression<Func<Item, bool>> predicate);
        Task<Item> AddItemAsync(Item data);
        Task UpdateItemAsync(int id, Item data);
        Task DeleteItemAsync(int id);
    }
}