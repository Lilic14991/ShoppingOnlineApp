using Microsoft.EntityFrameworkCore;
using ShopOnline.Api.Data;
using ShopOnline.Api.Entities;
using ShopOnline.Api.Repositories.Contracts;

namespace ShopOnline.Api.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ShopOnlineDbContext _context;

        public ProductRepository(ShopOnlineDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductCategory>> GetCategories()
        {
            var categories = await _context.ProductCategories.ToListAsync();

            return categories;
        }

        public async Task<ProductCategory> GetCategory(int id)
        {
            var category = await _context.ProductCategories.SingleOrDefaultAsync(c => c.Id == id);
            return category;
        }

        public async Task<Product> GetItem(int id)
        {
            var product = await _context.Products.FindAsync(id);
            return product;
        }

        public async Task<IEnumerable<Product>> GetItems()
        {
            var products = await _context.Products.ToListAsync();

            return products;
        }

        public async Task<IEnumerable<Product>> GetItemsByCategory(int id)
        {
            var products = await (from product in _context.Products
                                  where product.CategoryId == id
                                  select product).ToListAsync();
            return products;
        }
    }
}
