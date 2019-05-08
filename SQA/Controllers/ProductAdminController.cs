using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SQA.Abstraction;
using SQA.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SQA.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductAdminController : Controller
    {
        private IProductRepository repository;

        public ProductAdminController(IProductRepository repo)
        {
            repository = repo;
        }

        public ViewResult Index() => View(repository.Products);

        public ViewResult Edit(int productId) =>
            View(new CreateProduct
            {
                Product = repository.Products
                .FirstOrDefault(p => p.ProductID == productId)
            });

        [HttpPost]
        public async Task<IActionResult> Edit(CreateProduct CreateProduct)
        {
            if (ModelState.IsValid)
            {
                await repository.SaveProduct(CreateProduct);
                TempData["message"] = $"{CreateProduct.Product.Name} has been saved";
                return RedirectToAction("Index");
            }
            else
            {
                //TempData["message"] = "there is something wrong while trying to perform your request";
                return View(CreateProduct);
            }
        }

        public ViewResult Create() => View("Edit", new CreateProduct()
        {
            Product = new Product() { ProductID = 0 }
        });

        [HttpPost]
        public async Task<IActionResult> Delete(int productId)
        {
            Product deletedProduct = await repository.DeleteProduct(productId);
            if (deletedProduct != null)
            {
                TempData["message"] = $"{deletedProduct.Name} was deleted";
            }
            return RedirectToAction("Index");
        }
    }
}