using CatalogService.Data;
using CatalogService.DBContext;
using CatalogService.Interfaces;
using CatalogService.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogService
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services
                .AddEndpointsApiExplorer()
                .AddSwaggerGen(x => x.EnableAnnotations())
                .AddScoped<IItemService, ItemService>()
                .AddScoped<ICategoryService, CategoryService>()
                .AddDbContext<ApiContext>(options => options.UseInMemoryDatabase("TestDB"));

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            await InitializeDBAsync(app.Services);

            app.Run();
        }

        private static async Task InitializeDBAsync(IServiceProvider provider)
        {
            using var scope = provider.CreateAsyncScope();
            using var context = scope.ServiceProvider.GetService<ApiContext>();

            var categories = Enumerable.Range(1, 10)
                .Select(id => new Category { Name = $"Category #{id}" })
                .ToArray();

            await context.Categories.AddRangeAsync(categories);

            var random = new Random();
            var items = Enumerable.Range(1, 30)
               .Select(id => new Item
               {
                   Name = $"Item #{id}",
                   Category = categories[random.Next(1, categories.Length)]
               });
            await context.Items.AddRangeAsync(items);

            await context.SaveChangesAsync();
        }
    }
}