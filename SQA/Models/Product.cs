using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace SQA.Models
{
    public class Product
    {
        public int ProductID { get; set; }
        [Required(ErrorMessage = "Please enter a product name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please enter a description")]
        public string Description { get; set; }
        [Required]
        [Range(0.01, double.MaxValue,
        ErrorMessage = "Please enter a positive price")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Please specify a category")]
        public string Category { get; set; }
        public byte[] ImageData { get; set; }
        public string ImageExtention { get; set; }
    }
    public class CreateProduct
    {
        public Product Product { get; set; }
        public IFormFile Image { get; set; }
    }
}
