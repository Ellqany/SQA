using SQA.Models;
using System.Linq;

namespace SQA.Abstraction
{
    public interface IOrderRepository
    {
        IQueryable<Order> Orders { get; }
        void SaveOrder(Order order);
    }
}
