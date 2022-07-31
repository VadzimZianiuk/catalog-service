using CatalogService.Helpers;
using CatalogService.Interfaces;
using CatalogService.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq.Expressions;
using System.Net;

namespace CatalogService.Controllers
{
    /// <summary>
    /// Create, Read, Update and Delete items
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly IItemService _dataService;
        private readonly ILogger<ItemsController> _logger;

        public ItemsController(IItemService dataService, ILogger<ItemsController> logger)
        {
            _dataService = dataService;
            _logger = logger;
        }

        /// <summary>
        /// List of items
        /// </summary>
        /// <param name="name">Part of the item name</param>
        /// <param name="categoryName">Part of the category name</param>
        /// <returns>Found categories</returns>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(IEnumerable<Item>))]
        public IActionResult GetItems(string? name = null, string? categoryName = null)
        {
            _logger.LogDebug(nameof(GetItems));

            if (name == null && categoryName == null)
            {
                return Ok(_dataService.GetItems());
            }

            Expression<Func<Item, bool>> predicate = PredicateBuilder.True<Item>();
            if (name != null)
            {
                predicate = predicate.And((x) => x.Name.Contains(name, StringComparison.InvariantCultureIgnoreCase));
            }
            if (categoryName != null)
            {
                predicate = predicate.And((x) => x.Category.Name.Contains(categoryName, StringComparison.InvariantCultureIgnoreCase));
            }

            return Ok(_dataService.GetItems(predicate));
        }

        /// <summary>
        /// Item data
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Found item</returns>
        [HttpGet]
        [Route("{id:int}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(IEnumerable<Item>))]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        public IActionResult GetItem(int id)
        {
            _logger.LogDebug(nameof(GetItem));

            var item = _dataService.GetItems(x => x.Id == id).FirstOrDefault();
            return item is null
                ? NotFound()
                : Ok(item);
        }

        /// <summary>
        /// Create the item
        /// </summary>
        /// <param name="data">Item data</param>
        /// <returns>Created item</returns>
        [HttpPost]
        [SwaggerResponse((int)HttpStatusCode.Created, Type = typeof(Item))]
        public async Task<IActionResult> CreateItemAsync([FromBody] Item data)
        {
            _logger.LogDebug(nameof(CreateItemAsync));
            var item = await _dataService.AddItemAsync(data);
            return Created($"{Request.Scheme}://{Request.Host.Value}{Request.Path}/{item.Id}", item);
        }

        /// <summary>
        /// Update the item
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="item">Item data</param>
        [HttpPost]
        [Route("{id:int}")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "There is no item with same id")]
        public async Task<IActionResult> UpdateItemAsync(int id, [FromBody] Item item)
        {
            _logger.LogDebug(nameof(UpdateItemAsync));

            try
            {
                await _dataService.UpdateItemAsync(id, item);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex, nameof(UpdateItemAsync));
                return NotFound();
            }
        }

        /// <summary>
        /// Delete the Item
        /// </summary>
        /// <param name="id">Id</param>
        [HttpDelete]
        [Route("{id:int}")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteItemAsync(int id)
        {
            _logger.LogDebug(nameof(DeleteItemAsync));

            try
            {
                await _dataService.DeleteItemAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex, nameof(DeleteItemAsync));
                return NotFound();
            }
        }
    }
}