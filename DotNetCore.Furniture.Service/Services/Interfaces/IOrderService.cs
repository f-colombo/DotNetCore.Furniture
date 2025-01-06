using DotNetCore.Furniture.Domain.Common.Generics;
using DotNetCore.Furniture.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCore.Furniture.Service.Services.Interfaces
{
    public interface IOrderService
    {
        Task<NewResult<Order>> CreateOrderAsync(string userId, List<OrderItem> items);

        Task<NewResult<Order>> GetOrderByIdAsync(string orderId);

        Task<NewResult<IEnumerable<Order>>> GetOrdersByUserIdAsync(string userId);
    }
}
