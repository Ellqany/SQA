using Microsoft.AspNetCore.Mvc;
using SQA.Abstraction;
using SQA.Models;
using SQA.Models.ViewModels;
using System.Linq;

namespace SQA.Controllers
{
    public class ProductController : Controller
    {
        IProductRepository repository;
        int PageSize = 4;
        static int order = 1;

        public ProductController(IProductRepository repo) => repository = repo;

        public ViewResult List(string Category, int Page = 1) =>
            View(GetProduct(Category, Page));

        public IActionResult GetProducts(string Category, int Page = 1, int order = 1)
        {
            ProductController.order = order;
            return View(GetProduct(Category, Page));
        }

        ProductsListViewModel GetProduct(string Category, int Page)
        {
            var Products = repository.Products
                .Where(p => Category == null || p.Category == Category);
            if (order == 1)
            {
                Products = Products.OrderBy(x => x.Name);
            }
            else if (order == 2)
            {
                Products = Products.OrderBy(x => x.Price);
            }
            else if (order == 3)
            {
                Products = Products.OrderBy(x => x.Category);
            }
            else
            {
                Products = Products.OrderBy(x => x.ProductID);
            }
            return GetProduct(Products, Category, Page);
        }

        ProductsListViewModel GetProduct
            (IQueryable<Product> Products, string Category, int Page) =>
            new ProductsListViewModel
            {
                Products = Products.Skip((Page - 1) * PageSize).Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = Page,
                    ItemsPerPage = PageSize,
                    TotalItems = Category == null ?
                repository.Products.Count() :
                repository.Products.Where(e =>
                e.Category == Category).Count()
                },
                CurrentCategory = Category,
                Order = order
            };

        public FileContentResult GetImage(int productId)
        {
            Product prod = repository.Products
                .FirstOrDefault(p => p.ProductID == productId);
            if (prod != null)
            {
                return File(prod.ImageData, prod.ImageExtention);
            }
            else
            {
                return null;
            }
        }
    }
}