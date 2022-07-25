using CatalogService.Interfaces;
using CatalogService.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace CatalogService.Controllers
{
    /// <summary>
    /// Create, Read, Update and Delete categories
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _dataService;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(ICategoryService dataService, ILogger<CategoriesController> logger)
        {
            _dataService = dataService;
            _logger = logger;
        }

        /// <summary>
        /// List of categories
        /// </summary>
        /// <param name="name">Part of the category name</param>
        /// <returns>Found categories</returns>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(IEnumerable<Category>))]
        public IEnumerable<Category> GetCategories(string? name = null)
        {
            _logger.LogDebug(nameof(GetCategories));

            return name == null
                    ? _dataService.GetCategories()
                    : _dataService.GetCategories(x => x.Name.Contains(name, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// Category data
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Found category</returns>
        [HttpGet]
        [Route("{id:int}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(IEnumerable<Category>))]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        public IActionResult GetCategory(int id)
        {
            _logger.LogDebug(nameof(GetCategory));

            var category = _dataService.GetCategories(x => x.Id == id).FirstOrDefault();
            return category is null
                ? NotFound()
                : Ok(category);
        }

        /// <summary>
        /// Create the category
        /// </summary>
        /// <param name="category">Category data</param>
        /// <returns>Created category</returns>
        [HttpPost]
        [SwaggerResponse((int)HttpStatusCode.Created, Type = typeof(Category))]
        public async Task<IActionResult> CreateCategoryAsync([FromBody] Category category)
        {
            _logger.LogDebug(nameof(CreateCategoryAsync));

            var data = await _dataService.AddCategoryAsync(category);
            return Created($"{Request.Scheme}://{Request.Host.Value}{Request.Path}/{data.Id}", data);
        }

        /// <summary>
        /// Update the category
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="category">category data</param>
        [HttpPatch]
        [Route("{id:int}")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "There is no item with the same id")]
        public async Task<IActionResult> UpdateCategoryAsync(int id, [FromBody] Category category)
        {
            _logger.LogDebug(nameof(UpdateCategoryAsync));

            try
            {
                await _dataService.UpdateCategoryAsync(id, category);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex, nameof(UpdateCategoryAsync));
                return NotFound();
            }
        }

        /// <summary>
        /// Delete the category
        /// </summary>
        /// <param name="id">id</param>
        [HttpDelete]
        [Route("{id:int}")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "There is no item with the same id")]
        public async Task<IActionResult> DeleteCategoryAsync(int id)
        {
            _logger.LogDebug(nameof(DeleteCategoryAsync));
            try
            {
                await _dataService.DeleteCategoryAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex, nameof(DeleteCategoryAsync));
                return Ok();
            }
        }
    }
}