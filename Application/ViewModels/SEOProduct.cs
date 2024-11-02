using Domain.Models.Management;

namespace Application.ViewModels
{
    public class SEOProduct
    {
        public Product? Product { get; set; }        
        public IEnumerable<Product>? Products { get; set; }
        public IEnumerable<Product>? RelatedProducts { get; set; }        
        public IEnumerable<Category>? Categories { get; set; }
        public IEnumerable<Category>? CategoryForSearch { get; set; }
        public string STerm { get; set; } = "";
        public int CategoryId { get; set; } = 0;
        public double ProductPrice {  get { return Product?.ProductPrice ?? 0; } }
        public double DiscountedProductPrice { get { return Product?.DiscountProductprice ?? 0; } }
        public double DiscountedProductPricePercentage
        {
            get { return (int)Math.Ceiling(CalculateDiscountedPricePercentage(ProductPrice, DiscountedProductPrice)); }
        }

        #region
        private double CalculateDiscountedPricePercentage(double productPrice, double discountedProductPrice)
        {
            if(productPrice < 0)
            {
                throw new ArgumentException("Gia san pham phai lon hon hoac bang 0");
            }

            var result = ((productPrice - discountedProductPrice) / productPrice) * 100;

            return result;
        }
        #endregion
    }
}
