using Application.Interfaces.Management;
using Domain.Models.Management;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Management
{
    public class HomeService : IHomeService
    {
        private readonly ShelkobyPutDbContext _context;

        public HomeService(ShelkobyPutDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> Categories()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetAllproducts()
        {
            return await _context.Products.Include(p => p.Category).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetFeaturedProducts(int count)
        {
            return await _context.Products.Where(p => p.IsFeatured).Take(count).ToListAsync();
        }        

        public async Task<IEnumerable<Product>> GetProducts(string sTerm = "", int categoryId = 0)
        {
            sTerm = sTerm.ToLower();
            IEnumerable<Product> products = await (from product in _context.Products
                                                   join category in _context.Categories
                                                   on product.CategoryId equals category.Id
                                                   join stock in _context.Stocks
                                                   on product.Id equals stock.ProductId
                                                   into product_stocks
                                                   from productWithStock in product_stocks.DefaultIfEmpty()
                                                   where (string.IsNullOrWhiteSpace(sTerm) || (product != null && product.ProductName.ToLower().StartsWith(sTerm)))
                                                   && (categoryId == 0 || product.CategoryId == categoryId) // Filter by category if categoryId is provided
                                                   select new Product
                                                   {
                                                       Id = product.Id,
                                                       Pictures = product.Pictures,
                                                       ProductName = product.ProductName,
                                                       CategoryId = product.CategoryId,
                                                       ProductPrice = product.ProductPrice,
                                                       Category = product.Category,                                                       
                                                       Quantity = productWithStock == null ? 0 : productWithStock.Quantity
                                                   }
                                    ).Take(5).ToListAsync(); // Limit the results to 5 products

            return products;
        }
    }
}
