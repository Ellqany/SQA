using SQA.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SQA.Abstraction
{
    public interface IProductRepository
    {
        IQueryable<Product> Products { get; }
        Task SaveProduct(CreateProduct CreateProduct);
        Task<Product> DeleteProduct(int productID);
    }
}
