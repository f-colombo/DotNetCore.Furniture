using DotNetCore.Furniture.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCore.Furniture.Data.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> CreateOrderAsync(Order order);

        Task<Order> GetOrderByIdAsync(string orderId);

        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId);
    }
}
