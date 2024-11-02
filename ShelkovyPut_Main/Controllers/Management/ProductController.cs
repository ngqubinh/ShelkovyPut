using Application.DTOs.Request.Management;
using Application.Interfaces.Management;
using Application.ViewModels;
using Domain.Constants;
using Domain.Models.Management;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ShelkovyPut_Main.Controllers.Management
{
    public class ProductController : Controller
    {
        private readonly IProductService _product;
        private readonly ICategoryService _category;
        private readonly IHomeService _home;
        public ProductController(IProductService product, ICategoryService category, IHomeService home)
        {
            _product = product;
            _category = category;
            _home = home;
        }

        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<IActionResult> Product()
        {
            var products = await _product.GetAllProducts();
            return View(products);
        }
       

        [HttpGet]
        [Route("ProductsForUser")]
        public async Task<IActionResult> ProductForUser(string sterm = "", int categoryId = 0)
        {
            IEnumerable<Product> products = await _home.GetProducts(sterm, categoryId);
            if (!products.Any())
            {
                return NotFound("No products found");
            }

            IEnumerable<Category> categories = await _home.Categories();
            IEnumerable<Category> categoriesForSearch = await _home.Categories();

            var viewModel = new SEOProduct()
            {
                Products = products,               
                Categories = categories.Where(c => products.Any(p => p.CategoryId == c.Id)),
                CategoryForSearch = categoriesForSearch,
                STerm = sterm,
                CategoryId = categoryId
            };
            return View(viewModel);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> ProductDetail(int id, string sterm = "", int categoryId = 0)
        {
            var productResponse = await _product.GetProductById(id);
            if (productResponse == null)
            {
                return NotFound($"Khong co san pham {productResponse}");
            }

            if (categoryId == 0)
            {
                categoryId = productResponse.CategoryId;
            }

            IEnumerable<Product> products = await _home.GetProducts(sterm, categoryId);
            IEnumerable<Category> categories = await _home.Categories();

            var relatedProducts = await _product.GetAllRelatedProducts(productResponse.Id, 5);
            // if(!relatedProducts.Any())
            // {
            //     return NotFound("Loi");
            // }
            var viewModel = new SEOProduct()
            {
                Product = productResponse,
                Products = products,                
                RelatedProducts = relatedProducts,
                Categories = categories,
                STerm = sterm,
                CategoryId = categoryId
            };

            Console.WriteLine($"Product: {viewModel.Product.ProductName}");
            foreach (var rp in viewModel.RelatedProducts)
            {
                Console.WriteLine($"Related Product: {rp.ProductName}");
            }

            return viewModel == null ? NotFound() : View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> CreateProduct()
        {
            var categories = await _category.GetAllCategories();
            ViewData["Categories"] = new SelectList(categories, "Id", "CategoryName");
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateProduct(CreateProductRequest model)
        {
            var categories = await _category.GetAllCategories();
            ViewData["Categories"] = new SelectList(categories, "Id", "CategoryName");
            await _product.CreateProduct(model);
            return RedirectToAction(nameof(Product));
        }
    }
}
