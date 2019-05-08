using Microsoft.AspNetCore.Http;
using SQA.Abstraction;
using SQA.Models;
using SQA.Models.FacultyContext;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SQA.Concreate
{
    public class EFProductRepository : IProductRepository
    {
        private ApplicationDbContext context;
        public EFProductRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }
        public IQueryable<Product> Products => context.Products;

        public async Task SaveProduct(CreateProduct CreateProduct)
        {
            if (CreateProduct.Image != null)
            {
                var Image = await GetFileAsync(CreateProduct.Image);
                CreateProduct.Product.ImageExtention = Image.ImageExtention;
                CreateProduct.Product.ImageData = Image.ImageData;
            }
            if (CreateProduct.Product.ProductID == 0)
            {
                await context.Products.AddAsync(CreateProduct.Product);
            }
            else
            {
                Product dbEntry = context.Products
                .FirstOrDefault(p => p.ProductID == CreateProduct.Product.ProductID);
                if (dbEntry != null)
                {
                    dbEntry.Name = CreateProduct.Product.Name;
                    dbEntry.Description = CreateProduct.Product.Description;
                    dbEntry.Price = CreateProduct.Product.Price;
                    dbEntry.Category = CreateProduct.Product.Category;
                    dbEntry.ImageData = CreateProduct.Product.ImageData;
                    dbEntry.ImageExtention = CreateProduct.Product.ImageExtention;
                }
            }
            await context.SaveChangesAsync();
        }

        public async Task<Product> DeleteProduct(int productID)
        {
            Product dbEntry = context.Products
            .FirstOrDefault(p => p.ProductID == productID);
            if (dbEntry != null)
            {
                context.Products.Remove(dbEntry);
                await context.SaveChangesAsync();
            }
            return dbEntry;
        }


        #region Private Methods

        async Task<Image> GetFileAsync(IFormFile formFile)
        {
            MemoryStream ms = new MemoryStream();
            await formFile.OpenReadStream().CopyToAsync(ms);
            return new Image
            {
                ImageData = ms.ToArray(),
                ImageExtention = formFile.ContentType,
            };
        }

        #endregion

    }

}
